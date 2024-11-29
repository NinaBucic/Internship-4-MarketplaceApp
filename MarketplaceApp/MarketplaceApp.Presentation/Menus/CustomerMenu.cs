using MarketplaceApp.Data.Entities.Models;
using MarketplaceApp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Presentation.Menus
{
    public class CustomerMenu
    {
        private readonly Customer _customer;
        private readonly UserRepository _userRepository;
        private readonly ProductRepository _productRepository;

        public CustomerMenu(Customer customer, UserRepository userRepository, ProductRepository productRepository)
        {
            _customer = customer;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        public void Show()
        {
            var exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine(
                    $"Welcome, {_customer.Name.ToUpper()}! (Customer)\n\n"+
                    "1 - View Available Products\n"+
                    "2 - Buy Product\n"+
                    "3 - Return Purchased Product\n"+
                    "4 - Add Product to Favorites\n"+
                    "5 - View Purchase History\n"+
                    "6 - View Favorites List\n"+
                    "0 - Logout"
                    );

                var choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        //ViewAvailableProducts();
                        break;
                    case "2":
                        //BuyProduct();
                        break;
                    case "3":
                        //ReturnPurchasedProduct();
                        break;
                    case "4":
                        //AddToFavorites();
                        break;
                    case "5":
                        //ViewPurchaseHistory();
                        break;
                    case "6":
                        //ViewFavorites();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Press any key to try again...");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
