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

        public CustomerMenu(Customer customer, UserRepository userRepository)
        {
            _customer = customer;
            _userRepository = userRepository;
        }

        public void Show()
        {
            Console.WriteLine("Usla sam u customer menu!");
            Console.ReadKey();
        }
    }
}
