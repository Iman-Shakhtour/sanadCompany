namespace Sanad.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        
        public string TitleAr { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
         public string DescriptionAr { get; set; }
        public List<string>? Details { get; set; }
        public List<string>? DetailsAr { get; set; }
        public string? ImageUrl { get; set; }
    }
}
