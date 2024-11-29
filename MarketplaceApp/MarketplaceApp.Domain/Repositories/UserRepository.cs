using MarketplaceApp.Data.Entities.Models;
using MarketplaceApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Domain.Repositories
{
    public class UserRepository
    {
        private readonly MarketPlace _marketPlace;

        public UserRepository(MarketPlace marketPlace)
        {
            _marketPlace = marketPlace;
        }

        private bool IsEmailTaken(string email)
        {
            return _marketPlace.Users.Any(user => user.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public bool RegisterCustomer(string name, string email, double balance)
        {
            if (IsEmailTaken(email)) return false;

            var customer = new Customer(name, email, balance);
            _marketPlace.Users.Add(customer);
            return true;
        }

        public bool RegisterSeller(string name, string email)
        {
            if (IsEmailTaken(email)) return false;

            var seller = new Seller(name, email);
            _marketPlace.Users.Add(seller);
            return true;
        }

        public User? Login(string email)
        {
            return _marketPlace.Users.FirstOrDefault(user => user.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }
    }
}
