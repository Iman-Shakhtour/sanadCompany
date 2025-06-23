namespace Sanad.DTO
{
    public class CreateUpdateServiceDto
    {
        public string? Title { get; set; }
        public string? TitleAr { get; set; }

        public string? Description { get; set; }

        public string? DescriptionAr { get; set; }
        public IFormFile? Image { get; set; }
        public List<string>? Details { get; set; }
        public List<string>? DetailsAr { get; set; }
    }

}
