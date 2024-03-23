using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TeknoMarketData.Infrastructure;

namespace TeknoMarketData;

public class ProductImage
{
    public Guid UserId { get; set; }
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string Image { get; set; }

    public virtual Product? Product { get; set; }
}

public class ProductImageEntityTypeConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder
            .Property(p => p.Image)
            .IsUnicode(false);
    }
}