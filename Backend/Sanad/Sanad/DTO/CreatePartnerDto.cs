using Microsoft.AspNetCore.Http;

namespace Sanad.DTO
{
    public class CreatePartnerDto
    {
        public string Name { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public IFormFile? Logo { get; set; }
        public string? Website { get; set; }
    }
}