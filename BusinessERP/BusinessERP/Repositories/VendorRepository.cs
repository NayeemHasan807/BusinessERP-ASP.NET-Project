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
    }
}