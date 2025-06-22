using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sanad.Models;
using System.Text.Json;

namespace Sanad.Data
{
    public class ApplicationDbContext: IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {

        }
       protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    var stringListConverter = new ValueConverter<List<string>, string>(
        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
        v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null)
    );

    var stringListComparer = new ValueComparer<List<string>>(
        (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
        c => c == null ? 0 : c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
        c => c == null ? null : c.ToList()
    );

    // الحقل الأصلي (موجود)
    modelBuilder.Entity<Service>()
        .Property(s => s.Details)
        .HasConversion(stringListConverter)
        .Metadata.SetValueComparer(stringListComparer);

    // الجديد: DetailsEn و DetailsAr
    modelBuilder.Entity<Service>()
        .Property(s => s.DetailsEn)
        .HasConversion(stringListConverter)
        .Metadata.SetValueComparer(stringListComparer);

    modelBuilder.Entity<Service>()
        .Property(s => s.DetailsAr)
        .HasConversion(stringListConverter)
        .Metadata.SetValueComparer(stringListComparer);
}

        public DbSet<Service> Services { get; set; }
        public DbSet<Product> Products { get; set; }

    }
}
