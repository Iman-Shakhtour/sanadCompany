namespace Sanad.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        // عناوين ثنائية اللغة
        public string TitleEn { get; set; } = string.Empty;
        public string TitleAr { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        // أوصاف ثنائية اللغة
        public string DescriptionEn { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;

        public List<string>? Details { get; set; }
        // تفاصيل (قوائم) ثنائية اللغة
        public List<string>? DetailsEn { get; set; }
        public List<string>? DetailsAr { get; set; }

        public string? ImageUrl { get; set; }
    }
}
