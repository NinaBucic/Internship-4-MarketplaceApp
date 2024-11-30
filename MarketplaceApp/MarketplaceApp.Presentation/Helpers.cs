using MarketplaceApp.Data.Entities.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MarketplaceApp.Presentation
{
    public static class Helpers
    {
        public static bool IsNumeric(string value)
        {
            return double.TryParse(value, out var result);
        }

        public static string StringValidation(string prompt)
        {
            Console.Write(prompt);
            var stringInput = "";
            stringInput = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(stringInput) || IsNumeric(stringInput))
            {
                Console.Write("Invalid input! Please try again: ");
                stringInput = Console.ReadLine();
            }
            return stringInput;
        }

        public static bool YesNoValidation(string prompt)
        {
            Console.Write(prompt);
            var stringInput = "";
            stringInput = Console.ReadLine()?.ToLower();
            while (stringInput != "yes" && stringInput != "no")
            {
                Console.Write("Invalid input! Please try again (yes/no): ");
                stringInput = Console.ReadLine()?.ToLower();
            }
            return stringInput == "yes";
        }

        public static string EmailValidation(string prompt)
        {
            Console.Write(prompt);
            var emailInput = "";
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            emailInput = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(emailInput) || !Regex.IsMatch(emailInput, emailPattern))
            {
                Console.Write("Invalid email! Please try again: ");
                emailInput = Console.ReadLine();
            }
            return emailInput;
        }

        public static int PositiveIntegerValidation(string prompt)
        {
            Console.Write(prompt);
            var intInput = 0;
            while (!int.TryParse(Console.ReadLine(), out intInput) || intInput <= 0)
            {
                Console.Write("Invalid input! Please enter a positive number: ");
            }
            return intInput;
        }

        public static double PositiveDoubleValidation(string prompt)
        {
            Console.Write(prompt);
            var doubleInput = 0.0;
            while (!double.TryParse(Console.ReadLine()?.Replace('.', ','), out doubleInput) || doubleInput <= 0)
            {
                Console.Write("Invalid input! Please enter a positive number: ");
            }
            return doubleInput;
        }

        public static string FormatAsUSD(double amount)
        {
            return amount.ToString("C", new CultureInfo("en-US"));
        }

        public static bool IsValid(PromoCode code)
        {
            return DateTime.Now <= code.ExpirationDate;
        }

        public static ProductCategory SelectProductCategory()
        {
            Console.WriteLine("Select product category: ");

            var categories = Enum.GetValues(typeof(ProductCategory)).Cast<ProductCategory>().ToList();

            for (int i = 0; i < categories.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {categories[i]}");
            }

            var choice = PositiveIntegerValidation("Enter category number: ") - 1;

            while (choice < 0 || choice >= categories.Count)
            {
                Console.WriteLine("Invalid selection. Please try again.");
                choice = PositiveIntegerValidation("Enter category number: ") - 1;
            }

            return categories[choice];
        }

        public static DateTime GetValidDateInput(string prompt)
        {
            Console.Write(prompt);
            DateTime date;
            while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out date))
            {
                Console.Write("Invalid date format. Please enter a valid date (YYYY-MM-DD): ");
            }
            return date;
        }
    }
}
