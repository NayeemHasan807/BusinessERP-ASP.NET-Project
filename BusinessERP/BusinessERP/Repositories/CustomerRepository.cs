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
        public List<Customer> GetBySearch(string searchkey, string status)
        {
            if (searchkey != null)
            {
                if (status != "All")
                {
                    var list1 = context.Customers.Where(x => x.CustomerName.Contains(searchkey)).Where(x => x.Status == status).ToList();
                    return list1;
                }
                else
                {
                    var list1 = context.Customers.Where(x => x.CustomerName.Contains(searchkey)).ToList();
                    return list1;
                }

            }
            else
            {
                if (status != "All")
                {
                    var list2 = context.Customers.Where(x => x.Status == status).ToList();
                    return list2;
                }
                else
                {
                    var list2 = context.Customers.ToList();
                    return list2;
                }
            }
        }
    }
}