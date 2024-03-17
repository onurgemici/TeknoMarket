using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeknoMarket.Models;
using System.Security.Claims;

namespace TeknoMarket.Components
{
    public class NavBarViewComponent : ViewComponent
    {
        private readonly ICatalogsService catalogsService;
        private readonly IProductsService productsService;

        public NavBarViewComponent(
            ICatalogsService catalogsService,
            IProductsService productsService
            )
        {
            this.catalogsService = catalogsService;
            this.productsService = productsService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = Guid.Parse(UserClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
            return View(new NavbarViewModel
            {
                Catalogs = await catalogsService.GetAll().Where(p => p.Enabled).ToListAsync()
            });
        }
    }
}
