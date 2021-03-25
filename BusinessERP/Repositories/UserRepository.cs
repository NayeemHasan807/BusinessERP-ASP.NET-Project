using BusinessERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessERP.Repositories
{
    public class UserRepository:Repository<User>
    {
        public User GetByUserName(string username)
        {
            return context.Users.Where(x => x.UserName == username).FirstOrDefault();
        }
        public void DeleteByUsername(string username)
        {
            context.Users.Remove(GetByUserName(username));
            context.SaveChanges();
        }
    }
}