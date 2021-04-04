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
            if(collection["UserName"]!=null & collection["Password"]!=null)
            {
                var user = userrepo.GetByUserName(collection["UserName"]);
                if(user!=null)
                {
                    if (user.UserName == collection["UserName"] & user.Password == collection["Password"])
                    {
                        Session["UserName"] = collection["UserName"];
                        Session["UserType"] = user.UserType;
                        Session["Status"] = user.UserStatus;
                        Session["LoginStatus"] = "Ok";
                        if(Session["UserType"].ToString() == "Admin")
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        else if (Session["UserType"].ToString() == "Support")
                        {
                            return RedirectToAction("Index", "Support");
                        }
                        else if (Session["UserType"].ToString() == "Customer")
                        {
                            if(Session["Status"].ToString() == "Active")
                            {
                                return RedirectToAction("Index", "Customer");
                            }
                            else
                            {
                                TempData["Error"] = "Your account is temporarily blocked. Please contact with our support team.";
                                return RedirectToAction("Login", "Home");
                            }
                        }
                        else
                        {
                            TempData["Error"] = "You dont have permission to access on the site at this moment.";
                            return RedirectToAction("Login", "Home");
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