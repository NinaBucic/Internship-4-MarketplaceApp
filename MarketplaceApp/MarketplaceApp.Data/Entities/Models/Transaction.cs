using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Data.Entities.Models
{
    public class Transaction
    {
        private static int id = 0;
        public int InstanceId { get; private set; }
        public int ProductId { get; set; }
        public Customer Customer { get; set; }
        public Seller Seller { get; set; }
        public Product Product { get; set; }
        public DateTime DateAndTime { get; private set; }
        public double Amount { get; set; }
        public bool IsReturned { get; set; }
        public DateTime? ReturnDateAndTime { get; set; }

        public Transaction(Product product, Customer customer, Seller seller, double amount)
        {
            InstanceId = ++id;
            ProductId = product.InstanceId;
            Customer = customer;
            Seller = seller;
            Product = product;
            DateAndTime = DateTime.Now;
            Amount = amount;
            IsReturned = false;
        }
    }
}
