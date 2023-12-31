using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TeknoMarketData;

namespace TeknoMarket;

public abstract class ControllerBase : Controller
{
    public Guid UserId => Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
}
