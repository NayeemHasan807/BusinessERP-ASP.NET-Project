using BusinessERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessERP.Repositories
{
    public class CustomerRepository:Repository<Customer>
    {
        public Customer GetByUserName(string username)
        {
            return context.Customers.Where(x => x.UserName == username).FirstOrDefault();
        }
    }
}