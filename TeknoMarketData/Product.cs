using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using TeknoMarketData.Infrastructure;

namespace TeknoMarketData;

public class Product : EntityBase
{

    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int DiscountRate { get; set; }
    public string? Image { get; set; }

    public virtual ICollection<Catalog> Catalogs { get; set; } = new HashSet<Catalog>();
    public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new HashSet<OrderDetail>();
    public virtual ICollection<ProductImage> ProductImages { get; set; } = new HashSet<ProductImage>(); 
    public virtual ICollection<ShoppingCartItem> ShoppingCartItems { get; set; } = new HashSet<ShoppingCartItem>();

    public ICollection<Favorite> Favorites { get; set; } = new HashSet<Favorite>();


    [NotMapped]
    public decimal DiscountedPrice => Price - (Price * DiscountRate / 100.0m);

}

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder
            .HasIndex(p => new { p.Name });
        builder
            .Property(p => p.Price)
            .HasPrecision(18, 4);
        builder
            .Property(p => p.Image)
            .IsUnicode(false);

        builder
           .HasMany(p => p.Comments)
           .WithOne(p => p.Product)
           .HasForeignKey(p => p.ProductId)
           .OnDelete(DeleteBehavior.Cascade);

        builder
          .HasMany(p => p.OrderDetails)
          .WithOne(p => p.Product)
          .HasForeignKey(p => p.ProductId)
          .OnDelete(DeleteBehavior.Restrict);

        builder
          .HasMany(p => p.ProductImages)
          .WithOne(p => p.Product)
          .HasForeignKey(p => p.ProductId)
          .OnDelete(DeleteBehavior.Cascade);

        builder
          .HasMany(p => p.ShoppingCartItems)
          .WithOne(p => p.Product)
          .HasForeignKey(p => p.ProductId)
          .OnDelete(DeleteBehavior.Cascade);

        builder
  .HasMany(p => p.Favorites)
  .WithOne(p => p.Product)
  .HasForeignKey(p => p.ProductId)
  .OnDelete(DeleteBehavior.Cascade);
    }
}