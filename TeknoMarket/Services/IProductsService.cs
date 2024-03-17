using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using TeknoMarketData;
using TeknoMarketServices;
using TeknoMarketServices.Responses;
using TeknoMarket.Models;


namespace TeknoMarket;


public interface IProductsService
{
    IQueryable<Product> GetAll();

    Task<Product?> GetById(Guid id);

    Task<Product?> GetByIdWithCatalogs(Guid id);
    Task<List<ProductListResponse>> GetProductsAsync();

    Task<IEnumerable<ProductBoxViewModel>> GetBestSellersAsync(Guid? userId, int size = 10);
    Task<IEnumerable<ProductBoxViewModel>> GetByKeywords(string keywords, Guid? userId);

    Task AddToFavorites(Guid productId, Guid userId);

    Task<int> GetFavoriteCount(Guid userId);

    Task ClearFavorites(Guid userId);
    Task<List<Product>> GetFavoriteProducts(Guid userId);

    Task RemoveFromFavorites(Guid productId, Guid userId);

    Task Create(Product item);

    Task Create(string name, bool enabled, Guid userId, decimal price, int discountRate, string? description, string? image, IEnumerable<string>? images, IEnumerable<Guid> catalogs);

    Task Update(Product item, IEnumerable<Guid> catalogs, IEnumerable<string> images, IEnumerable<Guid> imagesToDelete);

    Task Delete(Guid id);



    string? GetProductImage(Guid id);
    byte[]? GetProductImageBytes(Guid id);

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

    public async Task Create(string name, bool enabled, Guid userId, decimal price, int discountRate, string? description, string? image, IEnumerable<string>? images, IEnumerable<Guid> catalogs)
    {
        var selectedCatalogs = context.Catalogs.Where(p => catalogs.Any(q => q == p.Id)).ToList();
        await Create(new Product
        {
            UserId = userId,
            Name = name,
            Enabled = enabled,
            Price = price,
            DiscountRate = discountRate,
            Description = description,
            Image = image,
            ProductImages = images?.Select(p => new ProductImage { UserId = userId, Image = p }).ToList(),
            Catalogs = selectedCatalogs
        });
    }
    public async Task Update(Product item, IEnumerable<Guid> catalogs, IEnumerable<string> images, IEnumerable<Guid> imagesToDelete)
    {
        var selectedCatalogs = context.Catalogs.Where(p => catalogs.Any(q => q == p.Id)).ToList();
        item.Catalogs.Clear();
        item.Catalogs = selectedCatalogs;
        images?.Select(p => new ProductImage { Image = p }).ToList().ForEach(item.ProductImages.Add);
        context.ProductImages.Where(p => imagesToDelete.Any(q => q == p.Id)).ExecuteDelete();
        context.Products.Update(item);

        await context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var item = await GetById(id);
        if (item is null)
            throw new Exception("Katalog bulunamadı");
        context.Products.Remove(item);
        await context.SaveChangesAsync();
    }

    public IQueryable<Product> GetAll()
    {
        return context.Products.AsQueryable<Product>();
    }

    public Task<Product?> GetById(Guid id)
    {
        return context.Products.SingleOrDefaultAsync(p => p.Id == id);
    }
    public Task<Product?> GetByIdWithCatalogs(Guid id)
    {
        return context.Products.Include(p => p.ProductImages).Include(p => p.Catalogs).SingleOrDefaultAsync(p => p.Id == id);
    }

    public string? GetProductImage(Guid id)
    {
        return GetAll().Select(p => new { p.Id, p.Image }).SingleOrDefault(p => p.Id == id)?.Image;
    }

    public byte[]? GetProductImageBytes(Guid id)
    {
        return Convert.FromBase64String(GetProductImage(id).Replace("data:image/jpeg;base64,", ""));
    }
    public async Task<List<ProductListResponse>> GetProductsAsync()
    {
        return await context.Products
            .Select(p => new ProductListResponse(p.Id, p.Name, p.Price, p.Image, p.DiscountedPrice, p.DiscountRate)).ToListAsync();
    }

    public async Task<IEnumerable<ProductBoxViewModel>> GetBestSellersAsync(Guid? userId, int size = 10)
    {
        return await context
            .Products
            .OrderByDescending(p => p.OrderDetails.Sum(q => q.Quantity))
            .Take(size)
            .Select(p => new ProductBoxViewModel
            {
                Id = p.Id,
                Name = p.Name,
                DiscountedPrice = p.DiscountedPrice,
                DiscountRate = p.DiscountRate,
                Image = p.Image,
                Price = p.Price,
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductBoxViewModel>> GetByKeywords(string keywords, Guid? userId)
    {
        var searchKeywords = Regex.Split(keywords.ToLower(), @"\s+").ToList();

        return context
            .Products
            .AsNoTracking()
            .AsEnumerable()
            .Where(p => p.Enabled && searchKeywords.Any(q => p.Name.ToLower().Contains(q)))
            .Select(p => new ProductBoxViewModel
            {
                Id = p.Id,
                Name = p.Name,
                DiscountedPrice = p.DiscountedPrice,
                DiscountRate = p.DiscountRate,
                Image = p.Image,
                Price = p.Price,
            })
            .ToList();
    }

    public async Task AddToFavorites(Guid productId, Guid userId)
    {
        var user = await context.Users.SingleAsync(p => p.Id == userId) as Customer;
        var favorite = user.Favorites.Where(p => p.ProductId == productId).FirstOrDefault();
        if(favorite == null)
        {
            user.Favorites.Add(new Favorite { ProductId = productId, UserId = userId });
        }
        await context.SaveChangesAsync();
    }

    public async Task<int> GetFavoriteCount(Guid userId)
    {
        return await context
            .Favorites
            .CountAsync(p => p.UserId == userId);
    }

    public async Task<List<Product>> GetFavoriteProducts(Guid userId)
    {
        return await context
            .Favorites
            .AsNoTracking()
            .Include(p => p.Product)
            .Where(p => p.UserId == userId)
            .Select(p => p.Product!)
            .ToListAsync();
    }

    public async Task RemoveFromFavorites(Guid productId, Guid userId)
    {
        await context.Favorites.Where(p => p.ProductId == productId && p.UserId == userId).ExecuteDeleteAsync();
    }

    public async Task ClearFavorites(Guid userId)
    {
        await context.Favorites.Where(p => p.UserId == userId).ExecuteDeleteAsync();
    }
}