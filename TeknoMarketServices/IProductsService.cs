using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknoMarketData;
using TeknoMarketServices;


public interface IProductsService
{
    Task<List<Product>> GetAll();

    Task<Product?> GetById(Guid id);


    Task Create(Product item);

    Task Create(string name, bool enabled, Guid userId, decimal price, int discountRate, string? description, string? image);

    Task Update(Product item);

    Task Delete(Guid id);
}

public class ProductsService : IProductsService
{
    private readonly AppDbContext context;

    public ProductsService(
        AppDbContext context
        )
    {
        this.context = context;
    }

    public async Task Create(Product item)
    {
        item.DateCreated = DateTime.UtcNow;

        await context.Products.AddAsync(item);
        await context.SaveChangesAsync();
    }

    public async Task Create(string name, bool enabled, Guid userId, decimal price, int discountRate, string? description, string? image)
    {
        await Create(new Product
        {
            UserId = userId,
            Name = name,
            Enabled = enabled,
            Price = price,
            DiscountRate = discountRate,
            Description = description,
            Image = image
        });
    }

    public async Task Delete(Guid id)
    {
        var item = await GetById(id);
        if (item is null)
            throw new Exception("Katalog bulunamadı");
        context.Products.Remove(item);
        await context.SaveChangesAsync();
    }

    public Task<List<Product>> GetAll()
    {
        return context.Products.ToListAsync();
    }

    public Task<Product?> GetById(Guid id)
    {
        return context.Products.SingleOrDefaultAsync(p => p.Id == id);
    }

    public async Task Update(Product item)
    {
        context.Products.Update(item);
        await context.SaveChangesAsync();
    }
}