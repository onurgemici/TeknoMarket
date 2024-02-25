using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknoMarketData;

namespace TeknoMarketServices.Responses
{
    public record ProductsResponse(Guid Id,string Name, decimal Price, IEnumerable<Catalog> Catalogs, bool Enabled, string UserName, DateTime DateCreated, string Image, decimal DiscountedPrice, int DiscountRate,IEnumerable<string> CatalogName);
}
