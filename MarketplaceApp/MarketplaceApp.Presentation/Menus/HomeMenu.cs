using MarketplaceApp.Data.Entities.Models;
using MarketplaceApp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Presentation.Menus
{
    public class HomeMenu
    {
        private readonly UserRepository _userRepository;

        public HomeMenu(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void Show()
        {
            var exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine(
                    "Welcome to the Marketplace!\n\n" +
                    "1 - Register\n" +
                    "2 - Login\n" +
                    "0 - Exit"
                    );

                var choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        RegisterUser();
                        break;
                    case "2":
                        LoginUser();
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

        private void RegisterUser()
        {
            Console.Clear();
            Console.WriteLine("Register as:\n1 - Customer\n2 - Seller");
            var userType = Console.ReadLine();
            Console.Clear();

            switch (userType)
            {
                case "1":
                    RegisterCustomer();
                    break;
                case "2":
                    RegisterSeller();
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private void RegisterCustomer()
        {
            var name = Helpers.StringValidation("Enter your name: ");
            var email = Helpers.EmailValidation("Enter your email: ");
            var balance = Helpers.PositiveDoubleValidation("Enter your initial balance: ");
            Console.Clear();

            if (_userRepository.RegisterCustomer(name, email, balance))
                Console.WriteLine("Customer registered successfully!");
            else
                Console.WriteLine("Email is already taken. Registration failed.");
        }

        private void RegisterSeller()
        {
            var name = Helpers.StringValidation("Enter your name: ");
            var email = Helpers.EmailValidation("Enter your email: ");
            Console.Clear();

            if (_userRepository.RegisterSeller(name, email))
                Console.WriteLine("Seller registered successfully!");
            else
                Console.WriteLine("Email is already taken. Registration failed.");
        }

        private void LoginUser()
        {
            Console.Clear();
            var email = Helpers.EmailValidation("Enter your email: ");

            var user = _userRepository.Login(email);

            if (user == null)
            {
                Console.WriteLine("User not found. Press any key to return...");
                Console.ReadKey();
                return;
            }

            if (user is Customer customer)
                new CustomerMenu(customer, _userRepository).Show();
            else if (user is Seller seller)
                new SellerMenu(seller, _userRepository).Show();
        }
    }
}
