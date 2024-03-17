using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknoMarketData;

namespace TeknoMarketServices.Responses
{
    public record ProductListResponse(Guid Id,string Name, decimal Price, string Image, decimal DiscountedPrice, int DiscountRate);
}
