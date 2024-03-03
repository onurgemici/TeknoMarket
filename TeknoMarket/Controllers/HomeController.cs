using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeknoMarket.Models;
using TeknoMarketData;
using TeknoMarketServices;

namespace TeknoMarket.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;
    private readonly IProductsService _productsService;

    public HomeController(ILogger<HomeController> logger, AppDbContext context,IProductsService productsService)
    {
        _logger = logger;
        _context = context;
        _productsService = productsService;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.CarouselImages = await _context.CarouselImages.ToListAsync();
        return View(await _productsService.GetProductsAsync());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
