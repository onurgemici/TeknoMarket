using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeknoMarket.Models;
using TeknoMarketData;
using TeknoMarket;
using Microsoft.AspNetCore.Authorization;


namespace TeknoMarket.Controllers;

public class HomeController : ControllerBase
{
    private readonly ICarouselImageService carouselImageService;
    private readonly IProductsService productsService;
    private readonly ICatalogsService catalogsService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(
        ICarouselImageService carouselImageService,
        IProductsService productsService,
        ICatalogsService catalogsService,
        ILogger<HomeController> logger)
    {
        this.carouselImageService = carouselImageService;
        this.productsService = productsService;
        this.catalogsService = catalogsService;
        _logger = logger;

    }

    public async Task<IActionResult> Index()
    {
        ViewBag.CarouselImages = await carouselImageService.GetAll().AsNoTracking().Where(p => p.Enabled && (DateTime.UtcNow > p.DateFirst || p.DateFirst == null) && (DateTime.UtcNow < p.DateEnd || p.DateEnd == null)).ToListAsync();
        ViewBag.BestSellers = await productsService.GetBestSellersAsync(UserId, 12);
        return View();
    }
    public async Task<IActionResult> Catalog(Guid id)
    {
        var model = await catalogsService
            .GetAll()
            .AsNoTracking()
            .Include(p => p.Products)
            .Select(p => new
            {
                p.Id,
                p.Name,
                Products = p.Products
                    .Where(p => p.Enabled)
                    .Select(q => new ProductBoxViewModel
                    {
                        Id = q.Id,
                        Name = q.Name,
                        DiscountedPrice = q.DiscountedPrice,
                        DiscountRate = q.DiscountRate,
                        Image = q.Image,
                        Price = q.Price,
                        IsInFavorites = q.Favorites.Any(r => r.UserId == UserId),

                    })
            })
            .SingleOrDefaultAsync(p => p.Id == id);
        return View(model);
    }

    public async Task<IActionResult> Search(string keywords)
    {
        return View(await productsService.GetByKeywords(keywords, UserId));
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public async Task<IActionResult> Favorites()
        {   
            return View(await productsService.GetFavoriteProducts(UserId!.Value));
        }
    [Authorize]
    public async Task<IActionResult> RemoveFromFavorites(Guid id)
    {
        await productsService.RemoveFromFavorites(id, UserId!.Value);
        return RedirectToAction(nameof(Favorites));
    }

    [Authorize]
    public async Task<IActionResult> ClearFavorites()
    {
        await productsService.ClearFavorites(UserId!.Value);
        return RedirectToAction(nameof(Index));
    }

}

