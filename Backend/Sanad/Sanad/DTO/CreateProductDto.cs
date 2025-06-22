using Microsoft.AspNetCore.Mvc;

namespace Sanad.DTO
{
    public class CreateProductDto
    {
        // العناوين
        [FromForm(Name = "titleEn")]
        public string TitleEn { get; set; } = string.Empty;

        [FromForm(Name = "titleAr")]
        public string TitleAr { get; set; } = string.Empty;

        // الوصف القصير
        [FromForm(Name = "descriptionEn")]
        public string DescriptionEn { get; set; } = string.Empty;

        [FromForm(Name = "descriptionAr")]
        public string DescriptionAr { get; set; } = string.Empty;

        // الوصف المطول
        [FromForm(Name = "longDescriptionEn")]
        public string? LongDescriptionEn { get; set; }

        [FromForm(Name = "longDescriptionAr")]
        public string? LongDescriptionAr { get; set; }

        // الصور
        [FromForm(Name = "image")]
        public IFormFile? Image { get; set; }

        [FromForm(Name = "thumbnails")]
        public List<IFormFile>? Thumbnails { get; set; }

        // بيانات إضافية
        [FromForm(Name = "year")]
        public int? Year { get; set; }

        [FromForm(Name = "category")]
        public string Category { get; set; } = string.Empty;

        [FromForm(Name = "tags")]
        public string? Tags { get; set; }

        [FromForm(Name = "buyLink")]
        public string? BuyLink { get; set; }

        [FromForm(Name = "detailsLink")]
        public string? DetailsLink { get; set; }

        [FromForm(Name = "demoLink")]
        public string? DemoLink { get; set; }
    }
}
