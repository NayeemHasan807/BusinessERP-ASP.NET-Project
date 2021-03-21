using BusinessERP.Models;
using BusinessERP.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessERP.Controllers
{
    public class HomeController : Controller
    {
        private UserRepository userrepo = new UserRepository();
        private RegistrationRequestRepository rrrepo = new RegistrationRequestRepository();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult About()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            if(Session["UserType"]!=null)
            {
                if (Session["UserType"].ToString() == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                    return View();
            }
            else
                return View();
        }
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(RegistrationRequest registration, string CPassword, HttpPostedFileBase image)
        {
            if(ModelState.IsValid)
            {
                if(registration.Password.ToString() == CPassword)
                {
                    var person = userrepo.GetByUserName(registration.UserName);
                    if (person == null)
                    {
                        if (image != null)
                        {
                            if (Path.GetExtension(image.FileName) == ".jpg" | Path.GetExtension(image.FileName) == ".png")
                            {
                                string name = Path.GetFileNameWithoutExtension(image.FileName);
                                string extention = Path.GetExtension(image.FileName);
                                name = name + DateTime.Now.ToString("yyyy-MM-dd") + extention;
                                string ProfilePicture = "~/Content/ProfilePictures/" + name;
                                string filepath = Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), name);
                                image.SaveAs(filepath);
                                registration.ProfilePicture = ProfilePicture;
                                rrrepo.Insert(registration);
                                TempData["Confirmation"] = "Your registration request submited successfully. We will verify your information soon and will give you confirmation.";
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                TempData["CPassword"] = CPassword;
                                TempData["Error2"] = "Profile picture must need to be type '.jpg' or '.png'";
                                return View(registration);
                            }
                        }
                        else
                        {
                            TempData["CPassword"] = CPassword;
                            TempData["Error2"] = "Must need to add a profile picture";
                            return View(registration);
                        }
                    }
                    else
                    {
                        TempData["CPassword"] = CPassword;
                        TempData["Error1"] = "This username is taken";
                        return View(registration);
                    }
                }
                else
                {
                    TempData["CPassword"] = CPassword;
                    TempData["PError"] = "Password & Confirm Password Must Need To Be Same!!!";
                    return View(registration);
                }
            }
            else
            {
                TempData["CPassword"] = CPassword;
                return View(registration);
            }
        }
    }
}