using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using TeknoMarketServices;

namespace TeknoMarket.Controllers;

public class FilesController : Controller
{
    private readonly IProductsService productsService;

    public FilesController(IProductsService productsService)
    {
        this.productsService = productsService;
    }

    [OutputCache(Duration = 86400)]
    public IActionResult ProductImage(Guid id)
    {
        return File(productsService.GetProductImageBytes(id), "image/jpeg");
    }
}

//bu sayfayı eklememişsin o yüzden oda burdaki controllerden imageyi çektiği için hata vermiş ayrıca hatalı olunca imageyi göstermemesinin nedeni ise yanlış pathi vermişsin 