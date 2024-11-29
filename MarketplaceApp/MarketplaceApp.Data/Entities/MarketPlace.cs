using MarketplaceApp.Data.Entities.Models;
using MarketplaceApp.Data.Seeds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Data.Entities
{
    public class MarketPlace
    {
        public List<User> Users { get; set; } = Seed.Users;
        public List<Product> Products { get; set; } = Seed.Products;
        public List<Transaction> Transactions { get; set; } = Seed.Transactions;
        public double Balance { get; set; }

        public MarketPlace()
        {
            Balance = 0.0;
        }
    }
}
