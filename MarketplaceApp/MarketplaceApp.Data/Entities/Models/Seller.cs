namespace MarketplaceApp.Data.Entities.Models
{
    public class Seller : User
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public double Balance { get; set; }

        public Seller(string name, string email) : base(name, email) 
        {
            Balance = 0.0;
        }
    }
}
