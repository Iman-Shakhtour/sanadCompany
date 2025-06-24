namespace Sanad.Models
{
    public class Partner
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }
        public string? Website { get; set; }
    }
}