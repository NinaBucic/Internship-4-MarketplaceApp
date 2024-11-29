using MarketplaceApp.Data.Entities.Models;
using MarketplaceApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
