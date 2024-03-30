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

    Task<int> GetShoppingCartCount(Guid userId);

    Task<List<ShoppingCartItem>> GetShoppingCart(Guid userId);

    Task AddToShoppingCart(Guid productId, Guid userId, int quantity);

    Task ClearShoppingCart(Guid userId);

    Task RemoveFromCart(Guid id);

    string? GetProductImage(Guid id);
    byte[]? GetProductImageBytes(Guid id);

    Task AddComment(Guid productId, Guid userId, string text, int rate);


    Task<List<GetCommentsViewModel>> GetAllNewCommentsAsync();
    Task<List<GetCommentsViewModel>> GetAllCommentsAsync();
    Task EnableComment(Guid id);
    Task RemoveComment(Guid id);


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

    public async Task AddToShoppingCart(Guid productId, Guid userId, int quantity)
    {
        var item = await context.ShoppingCartItems.SingleOrDefaultAsync(p => p.UserId == userId && p.ProductId == productId);
        if (item is null)
        {
            item = new ShoppingCartItem { ProductId = productId, UserId = userId, Quantity = quantity };
            context.ShoppingCartItems.Add(item);
        }
        else
        {
            item.Quantity += quantity;
            context.ShoppingCartItems.Update(item);
        }
        await context.SaveChangesAsync();

    }

    public async Task<int> GetShoppingCartCount(Guid userId)
    {
        return await context
            .ShoppingCartItems
            .CountAsync(p => p.UserId == userId);
    }

    public async Task<List<ShoppingCartItem>> GetShoppingCart(Guid userId)
    {
        return await context
            .ShoppingCartItems
            .AsNoTracking()
            .Include(p => p.Product)
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }

    public async Task RemoveFromCart(Guid id)
    {
        await context.ShoppingCartItems.Where(p => p.Id == id).ExecuteDeleteAsync();
    }

    public async Task ClearShoppingCart(Guid userId)
    {
        await context.ShoppingCartItems.Where(p => p.UserId == userId).ExecuteDeleteAsync();
    }

    public async Task AddComment(Guid productId, Guid userId, string text, int rate)
    {
        var comment = new Comment
        {
            Date = DateTime.UtcNow,
            DateCreated = DateTime.UtcNow,
            Enabled = false,
            ProductId = productId,
            UserId = userId,
            Text = text,
            Rate = rate
        };

        context.Comments.Add(comment);
        await context.SaveChangesAsync();

    }
    public async Task<List<GetCommentsViewModel>> GetAllNewCommentsAsync()
    {
        return await context
            .Comments
            .Include(p => p.User)
            .Include(p => p.Product)
            .OrderBy(p => p.DateCreated)
            .Where(p => !p.Enabled)
            .Select(p => new GetCommentsViewModel
            {
                Id = p.Id,
                Text = p.Text,
                Rating = p.Rate,
                Enabled = p.Enabled,
                Created = p.DateCreated,
                UserId = p.UserId,
                UserName = p.User!.Name,
                ProductId = p.ProductId,
                ProductName = p.Product!.Name
            }).ToListAsync();
    }
    public async Task<List<GetCommentsViewModel>> GetAllCommentsAsync()
    {
        return await context
            .Comments
            .Include(p => p.User)
            .Include(p => p.Product)
            .OrderBy(p => p.DateCreated)
            .Select(p => new GetCommentsViewModel
            {
                Id = p.Id,
                Text = p.Text,
                Rating = p.Rate,
                Enabled = p.Enabled,
                Created = p.DateCreated,
                UserId = p.UserId,
                UserName = p.User!.Name,
                ProductId = p.ProductId,
                ProductName = p.Product!.Name
            }).ToListAsync();
    }

    public async Task EnableComment(Guid id)
    {
        await context.Comments.Where(p => p.Id == id).ExecuteUpdateAsync(p => p.SetProperty(q => q.Enabled, true));
    }

    public async Task RemoveComment(Guid id)
    {
        await context.Comments.Where(p => p.Id == id).ExecuteDeleteAsync();

    }
}