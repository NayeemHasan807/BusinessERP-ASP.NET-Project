using BusinessERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessERP.Repositories
{
    public class VendorRepository:Repository<Vendor>
    {
        public Vendor GetByUserName(string username)
        {
            return context.Vendors.Where(x => x.UserName == username).FirstOrDefault();
        }
        public List<Vendor> GetBySearch(string searchkey, string status)
        {
            if (searchkey != null)
            {
                if(status!="All")
                {
                    var list1 = context.Vendors.Where(x => x.VendorName.Contains(searchkey)).Where(x => x.Status == status).ToList();
                    return list1;
                }
                else
                {
                    var list1 = context.Vendors.Where(x => x.VendorName.Contains(searchkey)).ToList();
                    return list1;
                }
                
            }
            else
            {
                if (status != "All")
                {
                    var list2 = context.Vendors.Where(x => x.Status == status).ToList();
                    return list2;
                }
                else
                {
                    var list2 = context.Vendors.ToList();
                    return list2;
                }
            }
        }
    }
}