using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sanad.Data;
using Sanad.DTO;
using Sanad.Models;
using System.Text.Json;

namespace Sanad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _env;

        public ServicesController(ApplicationDbContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }
        // GET: api/Services?lang=ar
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceDto>>> GetAll(
            [FromQuery] string lang = "en")
        {
            var services = await _dbContext.Services
                .Select(s => new ServiceDto
                {
                    Id = s.Id,
                    Title = lang == "ar" ? s.TitleAr : s.Title,
                    Description = lang == "ar" ? s.DescriptionAr : s.Description,
                    Details = lang == "ar" ? s.DetailsAr : s.Details
                })
                .ToListAsync();

            return Ok(services);
        }

        // GET: api/Services/5?lang=ar
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceDto>> GetById(
            int id,
            [FromQuery] string lang = "en")
        {
            var s = await _dbContext.Services.FindAsync(id);
            if (s == null) return NotFound("Service not found");

            var dto = new ServiceDto
            {
                Id = s.Id,
                Title = lang == "ar" ? s.TitleAr : s.Title,
                Description = lang == "ar" ? s.DescriptionAr : s.Description,
                Details = lang == "ar" ? s.DetailsAr : s.Details
            };

            return Ok(dto);
        }

        // POST: api/Services/createService
        [HttpPost("createService")]
        public async Task<ActionResult<Service>> CreateService([FromForm] CreateUpdateServiceDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // handle image upload
            string? fileName = null;
            if (dto.Image != null)
            {
                var folder = Path.Combine(_env.WebRootPath, "ServiceImages");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                fileName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
                var filePath = Path.Combine(folder, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await dto.Image.CopyToAsync(stream);
            }

            var service = new Service
            {
                Title = dto.Title,
                TitleAr = dto.TitleAr ?? string.Empty,
                Description = dto.Description,
                DescriptionAr = dto.DescriptionAr ?? string.Empty,
                Details = dto.Details,
                DetailsAr = dto.DetailsAr,
                ImageUrl = fileName != null ? $"/ServiceImages/{fileName}" : null
            };

            _dbContext.Services.Add(service);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = service.Id, lang = dto.TitleAr != null ? "ar" : "en" }, service);
        }

        // PUT: api/Services/5/updateService
        [HttpPut("{id}/updateService")]
        public async Task<IActionResult> UpdateService(int id, [FromForm] CreateUpdateServiceDto dto)
        {
            var service = await _dbContext.Services.FindAsync(id);
            if (service == null) return NotFound("Service not found");

            // update fields if provided
            if (!string.IsNullOrWhiteSpace(dto.Title))
                service.Title = dto.Title;
            if (!string.IsNullOrWhiteSpace(dto.TitleAr))
                service.TitleAr = dto.TitleAr;
            if (!string.IsNullOrWhiteSpace(dto.Description))
                service.Description = dto.Description;
            if (!string.IsNullOrWhiteSpace(dto.DescriptionAr))
                service.DescriptionAr = dto.DescriptionAr;
            if (dto.Details != null && dto.Details.Any())
                service.Details = dto.Details;
            if (dto.DetailsAr != null && dto.DetailsAr.Any())
                service.DetailsAr = dto.DetailsAr;

            // optionally replace image
            if (dto.Image != null)
            {
                // delete old
                if (!string.IsNullOrEmpty(service.ImageUrl))
                {
                    var oldPath = Path.Combine(_env.WebRootPath, service.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                }
                // save new
                var folder = Path.Combine(_env.WebRootPath, "ServiceImages");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                var newFile = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
                var newPath = Path.Combine(folder, newFile);
                using var stream = new FileStream(newPath, FileMode.Create);
                await dto.Image.CopyToAsync(stream);
                service.ImageUrl = $"/ServiceImages/{newFile}";
            }

            await _dbContext.SaveChangesAsync();
            return Ok(new { message = "Service updated successfully", service });
        }

        // DELETE: api/Services/5/deleteService
        [HttpDelete("{id}/deleteService")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _dbContext.Services.FindAsync(id);
            if (service == null) return NotFound("Service not found");

            // delete image file
            if (!string.IsNullOrEmpty(service.ImageUrl))
            {
                var path = Path.Combine(_env.WebRootPath, service.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            }

            _dbContext.Services.Remove(service);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Service deleted successfully" });
        }
    }
}
