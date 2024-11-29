using MarketplaceApp.Data.Entities.Models;
using MarketplaceApp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
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
                    $"Welcome, {_customer.Name.ToUpper()}! (Customer)\n"+
                    $"Your balance: {Helpers.FormatAsUSD(_customer.Balance)}\n\n"+
                    "1 - View Available Products\n" +
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
                        ViewAvailableProducts();
                        Console.WriteLine("\nPress any key to return...");
                        Console.ReadKey();
                        break;
                    case "2":
                        BuyProduct();
                        Console.WriteLine("Press any key to return...");
                        Console.ReadKey();
                        break;
                    case "3":
                        ReturnPurchasedProduct();
                        Console.WriteLine("\nPress any key to return...");
                        Console.ReadKey();
                        break;
                    case "4":
                        AddToFavorites();
                        Console.WriteLine("\nPress any key to return...");
                        Console.ReadKey();
                        break;
                    case "5":
                        ViewPurchaseHistory();
                        Console.WriteLine("\nPress any key to return...");
                        Console.ReadKey();
                        break;
                    case "6":
                        ViewFavorites();
                        Console.WriteLine("\nPress any key to return...");
                        Console.ReadKey();
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

        private void ViewAvailableProducts()
        {
            var availableProducts = _productRepository.GetAvailableProducts();
            Console.WriteLine("Available products:\n");

            if (availableProducts.Count == 0)
            {
                Console.WriteLine("No products currently available.");
                return;
            }

            foreach (var product in availableProducts)
            {
                Console.WriteLine($"- {product.Name}: {Helpers.FormatAsUSD(product.Price)}");
                Console.WriteLine($"  Description: {product.Description}");
            }
        }

        private double PromoCodeValidation(Product product)
        {
            var finalPrice = product.Price;
            if (!Helpers.YesNoValidation("Do you have a promo code? (yes/no): "))
            {
                Console.WriteLine("\nProceeding without discount.");
                return finalPrice;
            }

            var promoCodeInput = Helpers.StringValidation("Enter promo code: ");
            var promoCode = _productRepository.GetPromoCodeByCode(promoCodeInput);
            if (promoCode == null || !Helpers.IsValid(promoCode) || promoCode.ApplicableCategory != product.Category)
            {
                Console.WriteLine("\nInvalid or expired promo code. Proceeding without discount.");
                return finalPrice;
            }

            Console.WriteLine("\nProceeding with discount.");
            finalPrice -= finalPrice * (promoCode.DiscountPercentage / 100);
            Console.WriteLine($"Discount: {promoCode.DiscountPercentage}%");
            Console.WriteLine($"New price: {Helpers.FormatAsUSD(finalPrice)}\n");
            return finalPrice;
        }

        private void BuyProduct()
        {
            Console.WriteLine("Enter the ID of the product you wish to buy:\n");
            var products = _productRepository.GetAllProducts();

            if (products.Count == 0)
            {
                Console.WriteLine("No products available.");
                return;
            }

            foreach (var p in products)
            {
                Console.WriteLine($"ID: {p.InstanceId} - {p.Name} - Price: {Helpers.FormatAsUSD(p.Price)}");
            }

            var productId = Helpers.PositiveIntegerValidation("\nProduct ID: ");
            var product = _productRepository.GetProductById(productId);
            Console.Clear();

            if (product == null)
            {
                Console.WriteLine("Product not found.");
                return;
            }

            Console.WriteLine($"ID: {product.InstanceId} - {product.Name} - Price: {Helpers.FormatAsUSD(product.Price)}\n");

            if (product.Status != ProductStatus.ForSale)
            {
                Console.WriteLine("This product is not available for purchase.");
                return;
            }

            var finalPrice = PromoCodeValidation(product);

            if (_customer.Balance < finalPrice)
            {
                Console.WriteLine("\nInsufficient funds for this purchase.");
                return;
            }

            if (!Helpers.YesNoValidation("Buying? (yes/no): "))
            {
                Console.WriteLine("\nPurchase canceled!");
                return;
            }

            _productRepository.ProcessTransaction(_customer,product,finalPrice);
            Console.WriteLine("\nPurchase completed successfully!");
        }

        private void ReturnPurchasedProduct()
        {
            Console.WriteLine("Your purchase history:\n");
            if (_customer.PurchaseHistory.Count == 0)
            {
                Console.WriteLine("No purchased products to return.");
                return;
            }

            for (int i = 0; i < _customer.PurchaseHistory.Count; i++)
            {
                var product = _customer.PurchaseHistory[i];
                Console.WriteLine($"{i + 1}. {product.Name}");
            }

            var productIndex = Helpers.PositiveIntegerValidation("Enter the number of the product you want to return: ") - 1;
            Console.Clear();

            if (productIndex < 0 || productIndex >= _customer.PurchaseHistory.Count)
            {
                Console.WriteLine("Invalid product selection.");
                return;
            }

            var productToReturn = _customer.PurchaseHistory[productIndex];

            if (!Helpers.YesNoValidation($"Are you sure you want to return '{productToReturn.Name}'? (yes/no): "))
            {
                Console.WriteLine("\nReturn canceled.");
                return;
            }

            var refundAmount=_productRepository.ProcessReturn(_customer, productToReturn);
            Console.WriteLine($"\nThe product '{productToReturn.Name}' has been successfully returned.");
            Console.WriteLine($"Refund issued: {Helpers.FormatAsUSD(refundAmount)}. The product is now back for sale.");
        }

        private void AddToFavorites()
        {
            Console.WriteLine("Available Products:\n");
            var products = _productRepository.GetAllProducts();

            if (products.Count == 0)
            {
                Console.WriteLine("No products available.");
                return;
            }

            foreach (var p in products)
            {
                Console.WriteLine($"ID: {p.InstanceId} - {p.Name} - Price: {Helpers.FormatAsUSD(p.Price)}");
            }

            var productId = Helpers.PositiveIntegerValidation("\nEnter the ID of the product you want to add to favorites: ");
            var product = _productRepository.GetProductById(productId);
            Console.Clear();

            if (product == null)
            {
                Console.WriteLine("Product not found.");
                return;
            }

            if (_customer.Favorites.Contains(product))
            {
                Console.WriteLine($"The product '{product.Name}' is already in your favorites.");
                return;
            }

            _productRepository.AddToFavorites(_customer, product);
            Console.WriteLine($"Product '{product.Name}' has been added to your favorites.");
        }

        private void ViewPurchaseHistory()
        {
            Console.WriteLine("Your Purchase History:\n");

            var transactions = _userRepository.GetCustomerTransactions(_customer);

            if (transactions.Count == 0)
            {
                Console.WriteLine("You have not purchased any products yet.");
                return;
            }

            var output = new StringBuilder();

            foreach (var transaction in transactions)
            {
                if (!transaction.IsReturned)
                {
                    output.AppendLine($"- {transaction.Product.Name}: -{Helpers.FormatAsUSD(transaction.Amount)} (Purchased on {transaction.DateAndTime:G})\n");
                }
                else
                {
                    output.AppendLine($"- {transaction.Product.Name}: -{Helpers.FormatAsUSD(transaction.Amount)} (Purchased on {transaction.DateAndTime:G})");
                    var returnDate = transaction.ReturnDateAndTime.HasValue
                        ? $"{transaction.ReturnDateAndTime:G}"
                        : "Unknown return date";

                    output.AppendLine($"\t\t+{Helpers.FormatAsUSD(transaction.Amount * 0.8)} (Returned on {returnDate})\n");
                }
            }

            Console.WriteLine(output.ToString());
        }

        private void ViewFavorites()
        {
            Console.Clear();
            Console.WriteLine("Your Favorites:\n");

            if (_customer.Favorites.Count == 0)
            {
                Console.WriteLine("You have not added any products to your favorites.");
                return;
            }

            foreach (var product in _customer.Favorites)
            {
                Console.WriteLine($"- {product.Name}: {Helpers.FormatAsUSD(product.Price)} (Status: {product.Status})");
            }
        }


    }
}
