using MarketplaceApp.Data.Entities.Models;
using MarketplaceApp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Presentation.Menus
{
    public class SellerMenu
    {
        private readonly Seller _seller;
        private readonly UserRepository _userRepository;

        public SellerMenu(Seller seller, UserRepository userRepository)
        {
            _seller = seller;
            _userRepository = userRepository;
        }

        public void Show()
        {
            Console.WriteLine("Usla sam u selller menu!");
            Console.ReadKey();
        }
    }
}
