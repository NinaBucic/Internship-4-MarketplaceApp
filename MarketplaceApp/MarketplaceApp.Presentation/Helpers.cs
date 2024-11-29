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
            if (stringInput == "yes")
                return true;
            return false;
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
            while (!double.TryParse(Console.ReadLine(), out doubleInput) || doubleInput <= 0)
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
    }
}
