using BusinessERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessERP.Repositories
{
    public class CustomerLineItemRepository:Repository<CustomerLineItem>
    {
        public List<CustomerLineItem> GetByInvoiceId(int invoiceid)
        {
            return context.CustomerLineItems.Where(x => x.InvoiceId == invoiceid).ToList();
        }
    }
}