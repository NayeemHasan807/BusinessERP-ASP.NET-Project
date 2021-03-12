using BusinessERP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessERP.Controllers
{
    public class UserController : Controller
    {
        private UserRepository userrepo = new UserRepository();

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            if(collection["UserName"]!="" & collection["Password"]!="")
            {
                var user = userrepo.GetByUserName(collection["UserName"]);
                if(user!=null)
                {
                    if (user.UserName == collection["UserName"] & user.Password == collection["Password"])
                    {
                        Session["UserName"] = collection["UserName"];
                        Session["UserType"] = user.UserType;
                        if(Session["UserType"].ToString() == "Admin")
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                    }
                    TempData["Error"] = "Password is incorrect";
                    return RedirectToAction("Login", "Home");
                }
                TempData["Error"] = "This username does not exist";
                return RedirectToAction("Login", "Home");
            }
            TempData["Error"] = "Must need to enter the username and password";
            return RedirectToAction("Login", "Home");
        }
    }
}