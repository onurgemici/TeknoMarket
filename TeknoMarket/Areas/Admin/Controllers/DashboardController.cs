using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeknoMarketData;

namespace TeknoMarket.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrators,ProductAdministrators")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext context;

        public DashboardController(
            AppDbContext context
            )
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            var month = DateTime.Today.AddMonths(-1);
            var year = DateTime.Today.AddYears(-1);

            ViewBag.SalesMonthly = context
                .Orders
                .Where(p => p.Date > month)
                .AsEnumerable()
                .Sum(p => p.GrandTotal);

            ViewBag.SalesAnnually = context
                .Orders
                .Where(p => p.Date > year)
                .AsEnumerable()
                .Sum(p => p.GrandTotal);

            ViewBag.UsersCount = context
                .Users
                .Count();

            ViewBag.CommentsCount = context
                .Comments
                .Count(p => !p.Enabled);
            return View();
        }
    }
}
