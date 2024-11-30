using MarketplaceApp.Data.Entities.Models;
using MarketplaceApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Transactions;
using Transaction = MarketplaceApp.Data.Entities.Models.Transaction;

namespace MarketplaceApp.Domain.Repositories
{
    public class ProductRepository
    {
        private readonly MarketPlace _marketPlace;

        public ProductRepository(MarketPlace marketPlace)
        {
            _marketPlace = marketPlace;
        }

        public List<Product> GetAvailableProducts()
        {
            return _marketPlace.Products.Where(p => p.Status == ProductStatus.ForSale).ToList();
        }

        public List<Product> GetAllProducts()
        {
            return _marketPlace.Products;
        }

        public Product? GetProductById(int id)
        {
            return _marketPlace.Products.FirstOrDefault(p => p.InstanceId == id);
        }

        public PromoCode? GetPromoCodeByCode(string code)
        {
            return _marketPlace.PromoCodes.FirstOrDefault(c => c.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
        }

        public void ProcessTransaction(Customer customer, Product product, double finalPrice)
        {
            customer.Balance -= finalPrice;
            product.Status = ProductStatus.Sold;
            customer.PurchaseHistory.Add(product);

            double commission = finalPrice * 0.05;
            double sellerEarnings = finalPrice - commission;
            _marketPlace.Balance += commission;
            product.Seller.Balance += sellerEarnings;

            _marketPlace.Transactions.Add(new Transaction(product, customer, product.Seller, finalPrice));
        }

        public double ProcessReturn(Customer customer, Product product)
        {
            var transaction = _marketPlace.Transactions
                .First(t => t.Product.InstanceId == product.InstanceId && 
                            t.Customer == customer &&
                            !t.IsReturned);
            
            double refundAmount = transaction.Amount * 0.8;
            customer.Balance += refundAmount;
            transaction.Seller.Balance -= refundAmount;

            product.Status = ProductStatus.ForSale;
            transaction.IsReturned = true;
            transaction.ReturnDateAndTime = DateTime.Now;
            customer.PurchaseHistory.Remove(product);

            return refundAmount;
        }

        public void AddToFavorites(Customer customer, Product product)
        {
            customer.Favorites.Add(product);
        }

        public void AddProduct(Product product, Seller seller)
        {
            seller.Products.Add(product);
            _marketPlace.Products.Add(product);
        }

        public List<Product> GetProductsBySeller(Seller seller)
        {
            return _marketPlace.Products.Where(p => p.Seller == seller).ToList();
        }

        public List<Product> GetSoldProductsByCategory(Seller seller, ProductCategory category)
        {
            return _marketPlace.Transactions
                .Where(t => t.Seller == seller && t.Product.Status == ProductStatus.Sold && t.Product.Category == category)
                .Select(t => t.Product)
                .ToList();
        }

        public bool UpdateProductPrice(Seller seller, int productId, double newPrice)
        {
            var product = _marketPlace.Products.FirstOrDefault(p => p.InstanceId == productId && p.Seller == seller);

            if (product == null)
                return false;

            product.Price = newPrice;
            return true;
        }
    }
}
