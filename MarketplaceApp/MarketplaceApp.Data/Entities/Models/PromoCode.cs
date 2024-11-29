using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Data.Entities.Models
{
    public class PromoCode
    {
        public string Code { get; set; }
        public double DiscountPercentage { get; set; }
        public ProductCategory ApplicableCategory { get; set; }
        public DateTime ExpirationDate { get; set; }

        public PromoCode(string code, double discountPercentage, ProductCategory applicableCategory, DateTime expirationDate)
        {
            Code = code;
            DiscountPercentage = discountPercentage;
            ApplicableCategory = applicableCategory;
            ExpirationDate = expirationDate;
        }
    }

}
