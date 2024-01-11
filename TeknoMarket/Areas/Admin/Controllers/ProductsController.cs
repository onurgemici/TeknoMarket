using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TeknoMarket.Models;
using TeknoMarketData;
using TeknoMarketData.Infrastructure;
using TeknoMarketServices;
using SixLabors;
using Microsoft.AspNetCore.Hosting;
using IO = System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace TeknoMarket.Areas.Admin.Controllers;



[Area("Admin")]
[Authorize(Roles = "Administrators,ProductAdministrators")]
public class ProductsController : ControllerBase
{
    private readonly IProductsService productsService;
    private readonly ICatalogsService catalogsService;
    private readonly IFilesService filesService;
    private readonly IWebHostEnvironment webHostEnvironment;

    private readonly string entityName = "Katalog";
    public ProductsController(
        IProductsService productsService,
        ICatalogsService catalogsService,
        IFilesService filesService,
        IWebHostEnvironment webHostEnvironment
        )
    {
        this.productsService = productsService;
        this.catalogsService = catalogsService;
        this.filesService = filesService;
        this.webHostEnvironment = webHostEnvironment;
        this.catalogsService = catalogsService;
    }

    [Authorize(Roles = "Administrators,ProductAdministrators,OrderAdministrators")]
    public IActionResult Index()
    {
        var result = productsService.GetAll().Include(p => p.User).Include(p => p.Catalogs).ToList();
        return View(result);
    }
    public async Task<IActionResult> Create()
    {
        ViewBag.Catalogs = (await catalogsService.GetAll().Where(p => p.Enabled).OrderBy(p => p.Name).Select(p => new { p.Id, p.Name }).ToListAsync()).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
        return View(new ProductViewModel { Enabled = true, DiscountRate = "0" });
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductViewModel model)
    {
        var image = model.Image is not null ? await filesService.ResizeImageAsync(
                model.Image.OpenReadStream(),
                IO.File.Open(Path.Combine(webHostEnvironment.WebRootPath,  "lib","images", "logo.png"), FileMode.Open, FileAccess.Read, FileShare.Read),
                new SixLabors.ImageSharp.Size(800, 600)) : default;
        var images = model.Images is not null ? model.Images.Select(p => filesService.ResizeImageAsync(
                        p.OpenReadStream(),
                        IO.File.Open(Path.Combine(webHostEnvironment.WebRootPath, "lib", "images", "logo.png"), FileMode.Open, FileAccess.Read, FileShare.Read),
                        new SixLabors.ImageSharp.Size(800, 600)).Result) : default;

        await productsService.Create(
            model.Name,
            model.Enabled,
            UserId,
            description: model.Description,
            price: decimal.Parse(model.Price, CultureInfo.CreateSpecificCulture("tr-TR")),
            discountRate: int.Parse(model.DiscountRate),
            image: image,
            images: images,
            catalogs: model.Catalogs
            );
        TempData["success"] = $"{entityName} ekleme işlemi başarıyla tamamlanmıştır!";
        return RedirectToAction(nameof(Index));

    }
    public async Task<IActionResult> Edit(Guid id)
    {
        ViewBag.Catalogs = (await catalogsService.GetAll().Where(p => p.Enabled).OrderBy(p => p.Name).Select(p => new { p.Id, p.Name }).ToListAsync()).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
        var item = await productsService.GetById(id);
        if (item is null)
            return RedirectToAction(nameof(Index));
        return View(new ProductViewModel
        {
            Name = item.Name,
            Enabled = item.Enabled,
            Description = item.Description,
            Price =  item.Price.ToString(),
            DiscountRate =  item.DiscountRate.ToString(),
            //Image = item.Image,
            //Images = item.ProductImages,
            Catalogs = item.Catalogs.Select(x => x.Id)
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, ProductViewModel model)
    {
        var item = await productsService.GetById(id);

        item.Name = model.Name;
        item.Enabled = model.Enabled;

        TempData["success"] = $"{entityName} güncelleme işlemi başarıyla tamamlanmıştır!";

        await productsService.Update(item);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        await productsService.Delete(id);
        TempData["success"] = $"{entityName} silme işlemi başarıyla tamamlanmıştır!";
        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> SelectedCatalogs(Guid id)
    {
        var item = await productsService.GetById(id);
        if (item is null)
            throw new Exception("Product is not avaiable");
        return Json(item.Catalogs.Select(x => x.Id));
    }

}
