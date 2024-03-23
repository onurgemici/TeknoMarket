using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TeknoMarketData.Infrastructure;

namespace TeknoMarketData;

public class Catalog : EntityBase
{

    public string Name { get; set; }


    public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
}

public class CatalogEntityTypeConfiguration : IEntityTypeConfiguration<Catalog>
{
    public void Configure(EntityTypeBuilder<Catalog> builder)
    {
        builder
            .HasIndex(p => new { p.Name })
            .IsUnique();
    }
}

