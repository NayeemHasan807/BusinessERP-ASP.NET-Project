using BusinessERP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessERP.Controllers
{
    public class CompanyProductController : BaseController
    {
        private CompanyProductRepository comprodrepo = new CompanyProductRepository();
        public bool CheckIfAdmin()
        {
            if (HttpContext.Session["UserType"].ToString() == "Admin")
                return true;
            else
                return false;
        }
        public bool CheckIfCustomer()
        {
            if (HttpContext.Session["UserType"].ToString() == "Customer")
                return true;
            else
                return false;
        }
        
        [HttpGet]
        public ActionResult Index()
        {
            if (CheckIfCustomer())
            {
                return View(comprodrepo.GetAll());
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult ViewProduct(int id)
        {
            if (CheckIfCustomer())
            {
                return View(comprodrepo.GetById(id));
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult StockOut()
        {
            if(CheckIfAdmin())
            {
                return View(comprodrepo.StockOut());
            }
            else
                return RedirectToAction("Login", "Home");

        }
        [HttpGet]
        public ActionResult LowStock()
        {
            if (CheckIfAdmin())
            {
                return View(comprodrepo.LowStock());
            }
            else
                return RedirectToAction("Login", "Home");
        }
    }
}