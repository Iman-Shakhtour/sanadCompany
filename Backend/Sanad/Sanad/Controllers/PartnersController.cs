using Microsoft.AspNetCore.Mvc;
using Sanad.Data;
using Sanad.DTO;
using Sanad.Models;

namespace Sanad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnersController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IWebHostEnvironment env;

        public PartnersController(ApplicationDbContext dbContext, IWebHostEnvironment env)
        {
            this.dbContext = dbContext;
            this.env = env;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var partners = dbContext.Partners.ToList();
            return Ok(partners);
        }

        [HttpPost("CreatePartner")]
        public async Task<IActionResult> CreatePartner([FromForm] CreatePartnerDto dto)
        {
            string? logoFileName = null;
            if (dto.Logo != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PartnerLogos");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                logoFileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Logo.FileName);
                var filePath = Path.Combine(uploadsFolder, logoFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Logo.CopyToAsync(stream);
                }
            }

            var partner = new Partner
            {
                Name = dto.Name,
                LogoUrl = logoFileName,
                Website = dto.Website
            };

            dbContext.Partners.Add(partner);
            await dbContext.SaveChangesAsync();

            return Ok(new { message = "Partner created successfully.", partner });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePartner(int id, [FromForm] CreatePartnerDto dto)
        {
            var partner = await dbContext.Partners.FindAsync(id);
            if (partner == null)
                return NotFound("Partner not found");

            if (!string.IsNullOrEmpty(dto.Name))
                partner.Name = dto.Name;

            if (!string.IsNullOrEmpty(dto.Website))
                partner.Website = dto.Website;

            if (dto.Logo != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PartnerLogos");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // حذف الشعار القديم إذا وجد
                if (!string.IsNullOrEmpty(partner.LogoUrl))
                {
                    var oldPath = Path.Combine(uploadsFolder, partner.LogoUrl);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                var logoFileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Logo.FileName);
                var filePath = Path.Combine(uploadsFolder, logoFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Logo.CopyToAsync(stream);
                }

                partner.LogoUrl = logoFileName;
            }

            await dbContext.SaveChangesAsync();
            return Ok(new { message = "Partner updated successfully.", partner });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePartner(int id)
        {
            var partner = await dbContext.Partners.FindAsync(id);
            if (partner == null)
                return NotFound("Partner not found");

            // حذف الشعار من السيرفر إذا وجد
            if (!string.IsNullOrEmpty(partner.LogoUrl))
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PartnerLogos");
                var logoPath = Path.Combine(uploadsFolder, partner.LogoUrl);
                if (System.IO.File.Exists(logoPath))
                    System.IO.File.Delete(logoPath);
            }

            dbContext.Partners.Remove(partner);
            await dbContext.SaveChangesAsync();

            return Ok(new { message = "Partner deleted successfully." });
        }
    }
}