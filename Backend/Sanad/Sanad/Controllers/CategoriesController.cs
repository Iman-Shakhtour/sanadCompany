using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sanad.Data;
using Sanad.Models;
using Sanad.DTO;

namespace Sanad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/categories?lang=ar
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAll([FromQuery] string lang = "en")
        {
            var categories = await _context.Categories
                .Select(c => new
                {
                    Id = c.Id,
                    Name = lang == "ar" ? c.NameAr : c.NameEn,
                    Code = c.Code // Assuming Code is part of the Category model

                })
                .ToListAsync();

            return Ok(categories);
        }

        // ✅ GET: api/categories/5?lang=ar
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetById(int id, [FromQuery] string lang = "en")
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound("Category not found");

            return Ok(new
            {
                category.Id,
                Name = lang == "ar" ? category.NameAr : category.NameEn
            });
        }

        // ✅ POST: api/categories
        [HttpPost("createCategory")]
        public async Task<IActionResult> Create([FromBody] CategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = new Category
            {
                NameEn = dto.NameEn,
                NameAr = dto.NameAr,
                Code = dto.Code 


            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Category created successfully", id = category.Id });
        }

        // ✅ PUT: api/categories/5/updateCategory
        [HttpPut("{id}/updateCategory")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryDto dto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound("Category not found");

            category.NameEn = dto.NameEn;
            category.NameAr = dto.NameAr;
            category.Code = dto.Code; // Assuming Code is part of CategoryDto

            await _context.SaveChangesAsync();
            return Ok(new { message = "Category updated successfully" });
        }

        // ✅ DELETE: api/categories/5/deleteCategory
        [HttpDelete("{id}/deleteCategory")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound("Category not found");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Category deleted successfully" });
        }
    }
}
