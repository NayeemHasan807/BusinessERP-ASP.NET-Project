using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessERP.Models
{
    public class UserMetaData
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string UserStatus { get; set; }
    }
}