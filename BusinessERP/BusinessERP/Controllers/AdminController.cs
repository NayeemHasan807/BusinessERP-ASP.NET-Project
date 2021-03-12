using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessERP.Controllers
{
    public class AdminController : Controller
    {
        public bool CheckAccess()
        {
            if (Session["UserType"].ToString() == "Admin")
                return true;
            else
                return false;
        }
        [HttpGet]
        public ActionResult Index()
        {
            if (CheckAccess())
            {
                return View();
            }
            else
                return RedirectToAction("Login", "Home");
        }
    }
}