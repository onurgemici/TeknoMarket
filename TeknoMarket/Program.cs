using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TeknoMarketData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder
    .Services
    .AddDbContext<AppDbContext>(config =>
    {
        var dbProvider = builder.Configuration.GetValue<string>("DbProvider");
        var cs = builder.Configuration.GetConnectionString(dbProvider!);
        switch (dbProvider)
        {
            case "PostgreSql":
                config.UseNpgsql(cs, p => p.MigrationsAssembly("MigrationsPostgreSql"));
                break;
            case "MySql":
                config.UseMySql(cs, ServerVersion.AutoDetect(cs), p => p.MigrationsAssembly("MigrationsMySql"));
                break;
            case "SqlServer":
            default:
                config.UseSqlServer(cs, p => p.MigrationsAssembly("MigrationsSqlServer"));
                break;
        }
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
