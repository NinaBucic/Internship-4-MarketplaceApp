using MarketplaceApp.Data.Entities.Models;
using MarketplaceApp.Data.Seeds;

namespace MarketplaceApp.Data.Entities
{
    public class MarketPlace
    {
        public List<User> Users { get; set; } = Seed.Users;
        public List<Product> Products { get; set; } = Seed.Products;
        public List<Transaction> Transactions { get; set; } = Seed.Transactions;
        public List<PromoCode> PromoCodes { get; set; } = Seed.PromoCodes;
        public double Balance { get; set; }

        public MarketPlace()
        {
            Balance = 0.0;
        }
    }
}
