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

        public List<Transaction> GetCustomerTransactions(Customer customer)
        {
            return _marketPlace.Transactions.Where(t => t.Customer == customer).ToList();
        }

        public double GetEarningsInDateRange(Seller seller, DateTime startDate, DateTime endDate)
        {
            var transactionsInRange = _marketPlace.Transactions
                .Where(t => t.Seller == seller && t.DateAndTime.Date >= startDate && t.DateAndTime.Date <= endDate)
                .ToList();

            var totalEarnings = 0.0;

            foreach (var transaction in transactionsInRange)
            {
                if (transaction.IsReturned)
                    totalEarnings -= transaction.Amount * 0.8;
                else
                    totalEarnings += transaction.Amount * 0.95;
            }

            return totalEarnings;
        }

    }
}
