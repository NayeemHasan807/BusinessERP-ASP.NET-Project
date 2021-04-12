using BusinessERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessERP.Repositories
{
    public class CustomerInvoiceRepository:Repository<CustomerInvoice>
    {
        public List<CustomerInvoice> GetAllByUserName(string username)
        {
            return context.CustomerInvoices.Where(x => x.CustomerUserName == username).ToList();
        }
    }
}