using Sanad.Models;

namespace Sanad.Data.Seeders
{
    public class PartnersSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (context.Partners.Any()) return;

            var partners = new List<Partner>
            {
                new Partner
                {
                    Name = "Microsoft",
                    LogoUrl = "https://upload.wikimedia.org/wikipedia/commons/4/44/Microsoft_logo.svg",
                    Website = "https://www.microsoft.com"
                },
                new Partner
                {
                    Name = "Apple",
                    LogoUrl = "https://upload.wikimedia.org/wikipedia/commons/f/fa/Apple_logo_black.svg",
                    Website = "https://www.apple.com"
                },
                new Partner
                {
                    Name = "Google",
                    LogoUrl = "https://upload.wikimedia.org/wikipedia/commons/2/2f/Google_2015_logo.svg",
                    Website = "https://www.google.com"
                }
            };

            context.Partners.AddRange(partners);
            context.SaveChanges();
        }
    }
}