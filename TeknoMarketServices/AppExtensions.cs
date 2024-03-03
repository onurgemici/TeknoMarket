using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknoMarket;

namespace TeknoMarketServices;
public static class AppExtensions
{
    public static IServiceCollection AddTeknoMarket(this IServiceCollection services)
    {

        services
            .AddScoped<ICatalogsService, CatalogsService>();
        services
         .AddScoped<IProductsService, ProductsService>();

        services
       .AddScoped<IFilesService, FileService>();
        services
    .AddScoped<ICarouselImageService, CarouselImageService>();


        return services;
    }
}
