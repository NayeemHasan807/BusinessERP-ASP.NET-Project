using BusinessERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessERP.Repositories
{
    public class CompanyProductRepository:Repository<CompanyProduct>
    {
        public List<CompanyProduct> StockOut()
        {
            return context.CompanyProducts.Where(x => x.Quantity == 0).ToList();
        }
        public List<CompanyProduct> LowStock()
        {
            return context.CompanyProducts.Where(x => x.Quantity > 0 && x.Quantity<10).ToList();
        }
    }
}