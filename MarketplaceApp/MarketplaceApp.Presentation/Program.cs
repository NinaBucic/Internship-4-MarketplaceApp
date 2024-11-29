using MarketplaceApp.Data.Entities;
using MarketplaceApp.Data.Seeds;
using MarketplaceApp.Domain.Repositories;
using MarketplaceApp.Presentation.Menus;

namespace MarketplaceApp.Presentation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Seed.Initialize();
            var marketPlace = new MarketPlace();
            var userRepository = new UserRepository(marketPlace);
            var homeMenu = new HomeMenu(userRepository);
            homeMenu.Show();
        }
    }
}
