using BusinessERP.Models;
using BusinessERP.Models.ViewModels;
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
        private CustomerLineItemRepository cuslirepo = new CustomerLineItemRepository();
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
        //To get all products
        [HttpGet]
        public ActionResult Index()
        {
            if (CheckIfCustomer())
            {
                Session["Place"]= "Index";
                return View(comprodrepo.GetAll());
            }
            else
                return RedirectToAction("Login", "Home");
        }
        //To get best selling products
        [HttpGet]
        public ActionResult BestSelling()
        {
            if (CheckIfCustomer())
            {
                Session["Place"] = "BestSelling";
                var lineItems = cuslirepo.GetAll();
                var pid = lineItems.Select(x => x.ProductId).Distinct();
                List<ReportViewModel> tpvm = new List<ReportViewModel>();
                foreach (var item in pid)
                {
                    ReportViewModel index = new ReportViewModel();
                    index.Product = comprodrepo.GetById(item);
                    index.Count = lineItems.Where(x => x.ProductId == item).Count();
                    tpvm.Add(index);
                }
                List<ReportViewModel> ntpvm = tpvm.OrderByDescending(x => x.Count).Take(5).ToList();
                List<CompanyProduct> cproduct = new List<CompanyProduct>();
                foreach(var item in ntpvm)
                {
                    cproduct.Add(item.Product);
                }
                return View(cproduct);
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            if (CheckIfCustomer())
            {
                if (collection["searchkey"] == null)
                {
                    TempData["searchkey"] = collection["searchkey"];
                    return View(comprodrepo.GetAll());
                }
                TempData["searchkey"] = collection["searchkey"];
                return View(comprodrepo.GetAllSearchedByName(collection["searchkey"]));
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