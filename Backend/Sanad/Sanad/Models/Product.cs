namespace Sanad.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        // أسماء ثنائية اللغة
        public string TitleEn { get; set; } = string.Empty;
        public string TitleAr { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        // أوصاف قصيرة ثنائية اللغة
        public string DescriptionEn { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;

        public string? LongDescription { get; set; }

        // وصف مطوّل ثنائي اللغة (إن كان مطلوباً)
        public string? LongDescriptionEn { get; set; }
        public string? LongDescriptionAr { get; set; }

        public string? ImageUrl { get; set; }
        public int Year { get; set; }
        public string Category { get; set; } = string.Empty;


        public string? Thumbnails { get; set; } // JSON أو comma-separated
        public string? Tags { get; set; }  
        public string? BuyLink { get; set; }
        public string? DetailsLink { get; set; }
        public string? DemoLink { get; set; }
    }
}
