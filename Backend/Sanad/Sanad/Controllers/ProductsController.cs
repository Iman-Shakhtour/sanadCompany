using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
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
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _env;

        public ProductsController(ApplicationDbContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }

        // GET: api/Products?lang=ar&year=2024&category=...
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll(
            [FromQuery] string lang = "en",
            [FromQuery] int? year = null,
            [FromQuery] string? category = null)
        {
            var query = _dbContext.Products.AsQueryable();

            if (year.HasValue)
                query = query.Where(p => p.Year == year.Value);

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

            var list = await query.ToListAsync();

            var dto = list.Select(p => new ProductDto
            {
                Id              = p.Id,
                Title           = lang == "ar" ? (p.TitleAr       ?? p.TitleEn)       : (p.TitleEn       ?? p.TitleAr),
                Description     = lang == "ar" ? (p.DescriptionAr ?? p.DescriptionEn) : (p.DescriptionEn ?? p.DescriptionAr),
                LongDescription = lang == "ar" ? (p.LongDescriptionAr ?? p.LongDescriptionEn)
                                               : (p.LongDescriptionEn ?? p.LongDescriptionAr),
                ImageUrl        = p.ImageUrl,
                Year            = p.Year,
                Category        = p.Category,
                BuyLink         = p.BuyLink,
                DetailsLink     = p.DetailsLink,
                DemoLink        = p.DemoLink,
                Tags            = p.Tags?.Split(',').ToList(),
                Thumbnails      = string.IsNullOrEmpty(p.Thumbnails)
                                  ? null
                                  : JsonSerializer.Deserialize<List<string>>(p.Thumbnails)
            })
            .ToList();

            return Ok(dto);
        }

        // GET: api/Products/{id}?lang=ar
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetById(
            int id,
            [FromQuery] string lang = "en")
        {
            var p = await _dbContext.Products.FindAsync(id);
            if (p == null)
                return NotFound(new { message = "Product not found" });

            var dto = new ProductDto
            {
                Id              = p.Id,
                Title           = lang == "ar" ? (p.TitleAr       ?? p.TitleEn)       : (p.TitleEn       ?? p.TitleAr),
                Description     = lang == "ar" ? (p.DescriptionAr ?? p.DescriptionEn) : (p.DescriptionEn ?? p.DescriptionAr),
                LongDescription = lang == "ar" ? (p.LongDescriptionAr ?? p.LongDescriptionEn)
                                               : (p.LongDescriptionEn ?? p.LongDescriptionAr),
                ImageUrl        = p.ImageUrl,
                Year            = p.Year,
                Category        = p.Category,
                BuyLink         = p.BuyLink,
                DetailsLink     = p.DetailsLink,
                DemoLink        = p.DemoLink,
                Tags            = p.Tags?.Split(',').ToList(),
                Thumbnails      = string.IsNullOrEmpty(p.Thumbnails)
                                  ? null
                                  : JsonSerializer.Deserialize<List<string>>(p.Thumbnails)
            };

            return Ok(dto);
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(
            [FromForm] CreateProductDto dto,
            [FromQuery] string lang = "en")
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.Image == null || dto.Image.Length == 0)
                return BadRequest(new { message = "Main image is required." });

            var uploadsFolder = Path.Combine(_env.WebRootPath, "ProductImages");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Image.CopyToAsync(stream);
            }

            var thumbnailsUrls = new List<string>();
            if (dto.Thumbnails != null)
            {
                foreach (var thumb in dto.Thumbnails.Where(t => t.Length > 0))
                {
                    var thumbName = Guid.NewGuid() + Path.GetExtension(thumb.FileName);
                    var thumbPath = Path.Combine(uploadsFolder, thumbName);
                    using (var stream = new FileStream(thumbPath, FileMode.Create))
                    {
                        await thumb.CopyToAsync(stream);
                    }
                    thumbnailsUrls.Add(thumbName);
                }
            }

            var product = new Product
            {
                TitleEn           = dto.TitleEn,
                TitleAr           = dto.TitleAr,
                DescriptionEn     = dto.DescriptionEn,
                DescriptionAr     = dto.DescriptionAr,
                LongDescriptionEn = dto.LongDescriptionEn,
                LongDescriptionAr = dto.LongDescriptionAr,
                Year              = dto.Year ?? 0,
                Category          = dto.Category,
                ImageUrl          = fileName,
                Thumbnails        = thumbnailsUrls.Any()
                                     ? JsonSerializer.Serialize(thumbnailsUrls)
                                     : null,
                Tags              = dto.Tags,
                BuyLink           = dto.BuyLink,
                DetailsLink       = dto.DetailsLink,
                DemoLink          = dto.DemoLink
            };

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            var resultDto = new ProductDto
            {
                Id              = product.Id,
                Title           = lang == "ar" ? product.TitleAr : product.TitleEn,
                Description     = lang == "ar" ? product.DescriptionAr : product.DescriptionEn,
                LongDescription = lang == "ar" ? product.LongDescriptionAr : product.LongDescriptionEn,
                ImageUrl        = product.ImageUrl,
                Year            = product.Year,
                Category        = product.Category,
                BuyLink         = product.BuyLink,
                DetailsLink     = product.DetailsLink,
                DemoLink        = product.DemoLink,
                Tags            = product.Tags?.Split(',').ToList(),
                Thumbnails      = thumbnailsUrls
            };

            return CreatedAtAction(nameof(GetById),
                new { id = product.Id, lang },
                resultDto);
        }

        // PUT: api/Products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(
            int id,
            [FromForm] CreateProductDto dto,
            [FromQuery] string lang = "en")
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
                return NotFound(new { message = "Product not found" });

            if (!string.IsNullOrEmpty(dto.TitleEn))
                product.TitleEn = dto.TitleEn;
            if (!string.IsNullOrEmpty(dto.TitleAr))
                product.TitleAr = dto.TitleAr;
            if (!string.IsNullOrEmpty(dto.DescriptionEn))
                product.DescriptionEn = dto.DescriptionEn;
            if (!string.IsNullOrEmpty(dto.DescriptionAr))
                product.DescriptionAr = dto.DescriptionAr;
            if (!string.IsNullOrEmpty(dto.LongDescriptionEn))
                product.LongDescriptionEn = dto.LongDescriptionEn;
            if (!string.IsNullOrEmpty(dto.LongDescriptionAr))
                product.LongDescriptionAr = dto.LongDescriptionAr;
            if (dto.Year.HasValue)
                product.Year = dto.Year.Value;
            if (!string.IsNullOrEmpty(dto.Category))
                product.Category = dto.Category;
            if (!string.IsNullOrEmpty(dto.Tags))
                product.Tags = dto.Tags;
            if (!string.IsNullOrEmpty(dto.BuyLink))
                product.BuyLink = dto.BuyLink;
            if (!string.IsNullOrEmpty(dto.DetailsLink))
                product.DetailsLink = dto.DetailsLink;
            if (!string.IsNullOrEmpty(dto.DemoLink))
                product.DemoLink = dto.DemoLink;

            if (dto.Thumbnails != null && dto.Thumbnails.Any())
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "ProductImages");
                var thumbnailsUrls = new List<string>();
                foreach (var thumb in dto.Thumbnails.Where(t => t.Length > 0))
                {
                    var thumbName = Guid.NewGuid() + Path.GetExtension(thumb.FileName);
                    var thumbPath = Path.Combine(uploadsFolder, thumbName);
                    using (var stream = new FileStream(thumbPath, FileMode.Create))
                    {
                        await thumb.CopyToAsync(stream);
                    }
                    thumbnailsUrls.Add(thumbName);
                }
                product.Thumbnails = JsonSerializer.Serialize(thumbnailsUrls);
            }

            if (dto.Image != null && dto.Image.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "ProductImages");
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    var oldPath = Path.Combine(uploadsFolder, product.ImageUrl);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }
                var fileName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await dto.Image.CopyToAsync(stream);
                product.ImageUrl = fileName;
            }

            await _dbContext.SaveChangesAsync();
            return Ok(new { message = "Product updated successfully" });
        }

        // DELETE: api/Products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
                return NotFound(new { message = "Product not found" });

            var uploadsFolder = Path.Combine(_env.WebRootPath, "ProductImages");
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                var imagePath = Path.Combine(uploadsFolder, product.ImageUrl);
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
            }

            if (!string.IsNullOrEmpty(product.Thumbnails))
            {
                var thumbs = JsonSerializer.Deserialize<List<string>>(product.Thumbnails);
                if (thumbs != null)
                {
                    foreach (var thumb in thumbs)
                    {
                        var thumbPath = Path.Combine(uploadsFolder, thumb);
                        if (System.IO.File.Exists(thumbPath))
                            System.IO.File.Delete(thumbPath);
                    }
                }
            }

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Product deleted successfully" });
        }
    }
}
