using BusinessERP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessERP.Controllers
{
    public class CustomerController : Controller
    {
        private CustomerRepository customerrepo = new CustomerRepository();
        //Customer Home
        [HttpGet]
        public ActionResult Index()
        {
            var customer = customerrepo.GetByUserName(Session["UserName"].ToString());
            return View(customer);
        }
    }
}