using System.Collections.Generic;

namespace Sanad.DTO
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string? LongDescription { get; set; }

        public string? ImageUrl { get; set; }

        public int Year { get; set; }

        public string Category { get; set; } = string.Empty;

        public string? BuyLink { get; set; }

        public string? DetailsLink { get; set; }

        public string? DemoLink { get; set; }

        public List<string>? Tags { get; set; }

        public List<string>? Thumbnails { get; set; }
    }
}
