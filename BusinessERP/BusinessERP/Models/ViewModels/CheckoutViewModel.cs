using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessERP.Models.ViewModels
{
    public class CheckoutViewModel
    {
        public List<CompanyProduct> CartProductList { get; set; }

        public double TotalPrice { get; set; }
        public double TotalPriceWithTax { get; set; }
    }
}