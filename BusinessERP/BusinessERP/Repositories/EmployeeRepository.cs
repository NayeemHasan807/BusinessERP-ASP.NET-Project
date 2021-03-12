using BusinessERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessERP.Repositories
{
    public class EmployeeRepository:Repository<Employee>
    {
        public Employee GetByUserName(string username)
        {
            return context.Employees.Where(x => x.UserName == username).FirstOrDefault();
        }
    }
}