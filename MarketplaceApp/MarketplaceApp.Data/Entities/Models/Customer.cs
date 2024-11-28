using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Data.Entities.Models
{
    public class Customer : User
    {
        public double Balance { get; set; }
        public List<Product> PurchaseHistory { get; set; } = new List<Product>();
        public List<Product> Favorites { get; set; } = new List<Product>();

        public Customer(string name, string email, double balance) : base(name, email)
        {
            Balance = balance;
        }
    }
}
