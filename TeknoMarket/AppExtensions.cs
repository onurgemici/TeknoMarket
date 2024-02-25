using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TeknoMarketData;

namespace TeknoMarket;

public static class AppExtensions
{
    public static IApplicationBuilder UseTeknoMarket(this IApplicationBuilder builder)
    {

        using var scope = builder.ApplicationServices.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        context.Database.Migrate();

        roleManager.CreateAsync(new Role { Name = "Administrators" }).Wait();
        roleManager.CreateAsync(new Role { Name = "ProductAdministrators" }).Wait();
        roleManager.CreateAsync(new Role { Name = "OrderAdministrators" }).Wait();
        roleManager.CreateAsync(new Role { Name = "Members" }).Wait();

        var user = new Manager
        {
            UserName = configuration.GetValue<string>("Security:DefaultUser:UserName"),
            Email = configuration.GetValue<string>("Security:DefaultUser:UserName"),
            Name = configuration.GetValue<string>("Security:DefaultUser:Name"),
            EmailConfirmed = true
        };

        userManager.CreateAsync(user, configuration.GetValue<string>("Security:DefaultUser:Password")).Wait(); ;
        userManager.AddToRoleAsync(user, "Administrators").Wait();
        var claimResult = userManager.AddClaimAsync(user, new Claim(ClaimTypes.GivenName, configuration.GetValue<string>("Security:DefaultUser:Name"))).Result;

        return builder;
    }
}
