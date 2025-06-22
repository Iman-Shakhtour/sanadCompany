using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sanad.Data;
using Sanad.DTO;
using Sanad.Models;

namespace Sanad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public ServicesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Services?lang=ar
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceDto>>> GetServices(
            [FromQuery] string lang = "en")
        {
            var list = await _dbContext.Services
                .Select(s => new ServiceDto
                {
                    Id          = s.Id,
                    Title       = lang == "ar" 
                                    ? (s.TitleAr       ?? s.TitleEn) 
                                    : (s.TitleEn       ?? s.TitleAr),
                    Description = lang == "ar" 
                                    ? (s.DescriptionAr ?? s.DescriptionEn) 
                                    : (s.DescriptionEn ?? s.DescriptionAr),
                    Details     = lang == "ar" 
                                    ? (s.DetailsAr     ?? s.DetailsEn) 
                                    : (s.DetailsEn     ?? s.DetailsAr),
                    ImageUrl    = s.ImageUrl
                })
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/Services/{id}?lang=ar
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceDto>> GetService(
            int id,
            [FromQuery] string lang = "en")
        {
            var s = await _dbContext.Services.FindAsync(id);
            if (s == null) return NotFound(new { message = "Service not found" });

            var dto = new ServiceDto
            {
                Id          = s.Id,
                Title       = lang == "ar" 
                                ? (s.TitleAr       ?? s.TitleEn) 
                                : (s.TitleEn       ?? s.TitleAr),
                Description = lang == "ar" 
                                ? (s.DescriptionAr ?? s.DescriptionEn) 
                                : (s.DescriptionEn ?? s.DescriptionAr),
                Details     = lang == "ar" 
                                ? (s.DetailsAr     ?? s.DetailsEn) 
                                : (s.DetailsEn     ?? s.DetailsAr),
                ImageUrl    = s.ImageUrl
            };

            return Ok(dto);
        }

        // POST: api/Services
        [HttpPost]
        public async Task<ActionResult<ServiceDto>> CreateService(
            [FromForm] CreateUpdateServiceDto dto,
            [FromQuery] string lang = "en")
        {
            string? fileName = null;
            if (dto.Image != null)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ServiceImages");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                fileName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
                var path = Path.Combine(folder, fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await dto.Image.CopyToAsync(stream);
            }

            var service = new Service
            {
                TitleEn       = dto.TitleEn,
                TitleAr       = dto.TitleAr,
                DescriptionEn = dto.DescriptionEn,
                DescriptionAr = dto.DescriptionAr,
                DetailsEn     = dto.DetailsEn,
                DetailsAr     = dto.DetailsAr,
                ImageUrl      = fileName != null ? $"/ServiceImages/{fileName}" : null
            };

            _dbContext.Services.Add(service);
            await _dbContext.SaveChangesAsync();

            var result = new ServiceDto
            {
                Id          = service.Id,
                Title       = lang == "ar" ? service.TitleAr : service.TitleEn,
                Description = lang == "ar" ? service.DescriptionAr : service.DescriptionEn,
                Details     = lang == "ar" ? service.DetailsAr : service.DetailsEn,
                ImageUrl    = service.ImageUrl
            };

            return CreatedAtAction(nameof(GetService),
                new { id = service.Id, lang }, result);
        }

        // PUT: api/Services/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(
            int id,
            [FromForm] CreateUpdateServiceDto dto,
            [FromQuery] string lang = "en")
        {
            var service = await _dbContext.Services.FindAsync(id);
            if (service == null) 
                return NotFound(new { message = "Service not found" });

            if (!string.IsNullOrWhiteSpace(dto.TitleEn))       service.TitleEn       = dto.TitleEn;
            if (!string.IsNullOrWhiteSpace(dto.TitleAr))       service.TitleAr       = dto.TitleAr;
            if (!string.IsNullOrWhiteSpace(dto.DescriptionEn)) service.DescriptionEn = dto.DescriptionEn;
            if (!string.IsNullOrWhiteSpace(dto.DescriptionAr)) service.DescriptionAr = dto.DescriptionAr;
            if (dto.DetailsEn    != null && dto.DetailsEn.Any()) service.DetailsEn = dto.DetailsEn;
            if (dto.DetailsAr    != null && dto.DetailsAr.Any()) service.DetailsAr = dto.DetailsAr;

            if (dto.Image != null)
            {
                if (!string.IsNullOrEmpty(service.ImageUrl))
                {
                    var old = Path.Combine("wwwroot", service.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(old))
                        System.IO.File.Delete(old);
                }

                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ServiceImages");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
                var path = Path.Combine(folder, fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await dto.Image.CopyToAsync(stream);
                service.ImageUrl = $"/ServiceImages/{fileName}";
            }

            await _dbContext.SaveChangesAsync();
            return Ok(new { message = "Service updated successfully" });
        }

        // DELETE: api/Services/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _dbContext.Services.FindAsync(id);
            if (service == null) 
                return NotFound(new { message = "Service not found" });

            if (!string.IsNullOrEmpty(service.ImageUrl))
            {
                var file = Path.Combine("wwwroot", service.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(file))
                    System.IO.File.Delete(file);
            }

            _dbContext.Services.Remove(service);
            await _dbContext.SaveChangesAsync();
            return Ok(new { message = "Service deleted successfully" });
        }
    }
}
