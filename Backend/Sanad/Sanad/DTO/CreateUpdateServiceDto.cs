namespace Sanad.DTO
{
    public class CreateUpdateServiceDto
    {
        // عناوين ثنائية اللغة
        public string TitleEn { get; set; } = string.Empty;
        public string TitleAr { get; set; } = string.Empty;

        // أوصاف ثنائية اللغة
        public string DescriptionEn { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;

        // تفاصيل ثنائية اللغة
        public List<string>? DetailsEn { get; set; }
        public List<string>? DetailsAr { get; set; }

        // رفع صورة جديدة (Optional)
        public IFormFile? Image { get; set; }
    }
}
