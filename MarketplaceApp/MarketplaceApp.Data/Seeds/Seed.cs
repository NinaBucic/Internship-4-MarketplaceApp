using MarketplaceApp.Data.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Data.Seeds
{
    public static class Seed
    {
        public static List<User> Users = new List<User>();
        public static List<Product> Products = new List<Product>();
        public static List<Transaction> Transactions = new List<Transaction>();
        public static List<PromoCode> PromoCodes = new List<PromoCode>();
        private static bool _initialized = false;

        public static void Initialize()
        {
            if (_initialized) return;
            _initialized = true;

            var customers = new List<Customer>
            {
                new Customer("Alice", "alice@example.com", 1500),
                new Customer("Bob", "bob@example.com", 2500),
                new Customer("Charlie", "charlie@example.com", 100)
            };

            var sellers = new List<Seller>
            {
                new Seller("Diana", "diana@example.com"),
                new Seller("Edward", "edward@example.com")
            };

            var products = new List<Product>
            {
                new Product("Laptop", "High-end gaming laptop", 1200, ProductCategory.Electronics, sellers[0]),
                new Product("Smartphone", "Latest model smartphone", 800, ProductCategory.Electronics, sellers[0]),
                new Product("Wrist Watch", "Luxury wrist watch", 300, ProductCategory.JewelryAndWatches, sellers[1]),
                new Product("Running Shoes", "Comfortable running shoes", 100, ProductCategory.ApparelAndAccessories, sellers[1])
            };

            sellers[0].Products.AddRange(products.Where(p => p.Seller == sellers[0]));
            sellers[1].Products.AddRange(products.Where(p => p.Seller == sellers[1]));

            var transactions = new List<Transaction>
            {
                new Transaction(products[0], customers[0], sellers[0],products[0].Price),
                new Transaction(products[2], customers[1], sellers[1],products[2].Price),
                new Transaction(products[3], customers[2], sellers[1], products[3].Price)
            };

            sellers[0].Balance += products[0].Price * 0.95;
            sellers[1].Balance += products[2].Price * 0.95;
            sellers[1].Balance += products[3].Price * 0.95;

            customers[0].Balance -= products[0].Price;
            customers[1].Balance -= products[2].Price;
            customers[2].Balance -= products[3].Price;

            customers[0].PurchaseHistory.Add(products[0]);
            customers[1].PurchaseHistory.Add(products[2]);
            customers[2].PurchaseHistory.Add(products[3]);

            products[0].Status = ProductStatus.Sold;
            products[2].Status = ProductStatus.Sold;
            products[3].Status = ProductStatus.Sold;

            PromoCodes.Add(new PromoCode("DISCOUNT10", 10, ProductCategory.Electronics, DateTime.Now.AddMonths(1)));
            PromoCodes.Add(new PromoCode("DISCOUNT20", 20, ProductCategory.ApparelAndAccessories, DateTime.Now.AddMonths(1)));

            Users.AddRange(customers.Cast<User>().Concat(sellers.Cast<User>()));
            Products.AddRange(products);
            Transactions.AddRange(transactions);
        }
    }
}
