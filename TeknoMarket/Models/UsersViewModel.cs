using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TeknoMarketData;

namespace TeknoMarket.Models
{
    public class UsersViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserAddress { get; set; }

        public virtual User User { get; set; }
    }
}
