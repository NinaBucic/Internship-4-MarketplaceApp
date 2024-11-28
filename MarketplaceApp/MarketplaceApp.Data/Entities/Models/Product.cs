using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Data.Entities.Models
{
    public class Product
    {
        private static int id = 0;
        public int InstanceId { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public ProductStatus Status { get; set; }
        public ProductCategory Category { get; set; }
        public Seller Seller { get; set; }

        public Product(string name, string description, double price, ProductCategory category, Seller seller)
        {
            InstanceId=++id;
            Name = name;
            Description = description;
            Price = price;
            Status = ProductStatus.ForSale;
            Category = category;
            Seller = seller;
        }
    }
}
