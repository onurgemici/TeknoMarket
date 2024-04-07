using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeknoMarket.Models;
using TeknoMarketData; // User ve AppDbContext sınıflarını içerir

namespace TeknoMarket.Areas.Admin.Controllers;



[Area("Admin")]
[Authorize(Roles = "Administrators,ProductAdministrators")]


public class UsersController : Controller
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _context.Users
     .OfType<Customer>()
     .Include(u => u.Addresses)
     .Include(u => u.Comments)
     .Include(u => u.Orders)
     .Include(u => u.ShoppingCartItems)
     .Include(u => u.Favorites)
     .ToListAsync();

        var usersViewModels = users.Select(u => new UsersViewModel
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,

        }).ToList();

        return View(usersViewModels);
    }
}
