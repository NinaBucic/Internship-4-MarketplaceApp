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
        private readonly ProductRepository _productRepository;

        public SellerMenu(Seller seller, UserRepository userRepository, ProductRepository productRepository)
        {
            _seller = seller;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        public void Show()
        {
            Console.WriteLine("Usla sam u selller menu!");
            Console.ReadKey();
        }
    }
}
