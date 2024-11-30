using MarketplaceApp.Data.Entities.Models;
using MarketplaceApp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine(
                    $"Welcome, {_seller.Name.ToUpper()}! (Seller)\n\n" +
                    "1 - Add Product\n" +
                    "2 - View All Owned Products\n" +
                    "3 - View Total Earnings\n" +
                    "4 - View Sold Products by Category\n" +
                    "5 - View Earnings in Date Range\n" +
                    "6 - Update Product Price\n" +
                    "0 - Logout"
                    );

                var choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        AddProduct();
                        Console.WriteLine("\nPress any key to return...");
                        Console.ReadKey();
                        break;
                    case "2":
                        ViewOwnedProducts();
                        Console.WriteLine("Press any key to return...");
                        Console.ReadKey();
                        break;
                    case "3":
                        ViewTotalEarnings();
                        Console.WriteLine("\nPress any key to return...");
                        Console.ReadKey();
                        break;
                    case "4":
                        ViewSoldProductsByCategory();
                        Console.WriteLine("\nPress any key to return...");
                        Console.ReadKey();
                        break;
                    case "5":
                        ViewEarningsInDateRange();
                        Console.WriteLine("\nPress any key to return...");
                        Console.ReadKey();
                        break;
                    case "6":
                        UpdateProductPrice();
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

        private void AddProduct()
        {
            Console.WriteLine("Add a New Product\n");

            var name = Helpers.StringValidation("Enter product name: ");
            var description = Helpers.StringValidation("Enter product description: ");
            var price = Helpers.PositiveDoubleValidation("Enter product price: ");
            Console.WriteLine();
            var category = Helpers.SelectProductCategory();
            Console.Clear();

            var newProduct = new Product(name, description, price, category, _seller);

            _productRepository.AddProduct(newProduct,_seller);

            Console.WriteLine($"Product '{newProduct.Name}' has been successfully added!");
        }

        private void ViewOwnedProducts()
        {
            Console.WriteLine("Your Products:\n ");

            var products = _productRepository.GetProductsBySeller(_seller);

            if (products.Count == 0)
            {
                Console.WriteLine("You have not added any products yet.");
                return;
            }

            foreach (var product in products)
            {
                Console.WriteLine(
                    $"- {product.Name}\n"+
                    $"  Description: {product.Description}\n"+
                    $"  Price: {Helpers.FormatAsUSD(product.Price)}\n"+
                    $"  Status: {product.Status}\n"
                    );
            }
        }

        private void ViewTotalEarnings()
        {
            Console.WriteLine($"Your total earnings from sales: {Helpers.FormatAsUSD(_seller.Balance)}");
        }

        private void ViewSoldProductsByCategory()
        {
            var selectedCategory = Helpers.SelectProductCategory();
            var soldProducts = _productRepository.GetSoldProductsByCategory(_seller, selectedCategory);
            Console.Clear();

            if (soldProducts.Count == 0)
            {
                Console.WriteLine($"No products have been sold in the '{selectedCategory}' category.");
                return;
            }

            Console.WriteLine($"Products sold in the '{selectedCategory}' category:\n");

            foreach (var product in soldProducts)
            {
                Console.WriteLine($"- {product.Name}: {Helpers.FormatAsUSD(product.Price)}");
            }
        }

        private void ViewEarningsInDateRange()
        {
            var startDate = Helpers.GetValidDateInput("Enter the start date (YYYY-MM-DD): ");
            var endDate = Helpers.GetValidDateInput("Enter the end date (YYYY-MM-DD): ");
            Console.Clear();

            if (endDate < startDate)
            {
                Console.WriteLine("End date cannot be earlier than start date.");
                return;
            }

            var totalEarnings = _userRepository.GetEarningsInDateRange(_seller, startDate, endDate);

            Console.WriteLine($"Total earnings from {startDate.ToShortDateString()} to {endDate.ToShortDateString()}: {Helpers.FormatAsUSD(totalEarnings)}");
        }

        private void UpdateProductPrice()
        {
            Console.WriteLine("Update Product Price:\n");
            Console.WriteLine("NOTE: If the product is currently sold, " +
                "the new price will be applied once the product is available again on the marketplace.\n");

            var products = _productRepository.GetProductsBySeller(_seller);

            if (products.Count == 0)
            {
                Console.WriteLine("You have no products to update.");
                return;
            }

            foreach (var product in products)
            {
                Console.WriteLine($"ID: {product.InstanceId} - {product.Name} - Price: {Helpers.FormatAsUSD(product.Price)}");
            }

            var productId = Helpers.PositiveIntegerValidation("\nEnter the product ID to update the price: ");
            Console.Clear();
            var productToUpdate = products.FirstOrDefault(p => p.InstanceId == productId);

            if (productToUpdate == null)
            {
                Console.WriteLine("Product not found.");
                return;
            }

            var newPrice = Helpers.PositiveDoubleValidation($"Enter the new price for the product '{productToUpdate.Name}': ");
            Console.Clear();

            if (_productRepository.UpdateProductPrice(_seller, productId, newPrice))
            {
                Console.WriteLine($"The price of {productToUpdate.Name} has been updated to {Helpers.FormatAsUSD(newPrice)}.");
            }
            else
            {
                Console.WriteLine("Unable to update the product price.");
            }
        }
    }
}
