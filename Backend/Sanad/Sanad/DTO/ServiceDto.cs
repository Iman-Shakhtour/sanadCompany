namespace Sanad.DTO
{
    public class ServiceDto
    {
        public int Id { get; set; }

        // يتم تعبئته من الـ Controller بناءً على ?lang=ar/en
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string>? Details { get; set; }

        // URL للصورة الجاهزة للعرض
        public string? ImageUrl { get; set; }
    }
}
