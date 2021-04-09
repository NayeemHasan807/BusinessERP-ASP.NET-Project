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
        private CompanyProductRepository comprodrepo = new CompanyProductRepository();
        private RequestToSupportRepository rtsrepo = new RequestToSupportRepository();
        
        public bool CheckIfBlocked()
        {
            if (HttpContext.Session["LoginStatus"].ToString() == "Blocked")
                return true;
            else
                return false;
        }

        [HttpGet]
        public ActionResult Products()
        {
            return View(comprodrepo.GetAll());
        }

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
        [HttpGet]
        public ActionResult ViewProduct(int id)
        {
            TempData["P"] = "You have to login first to view product details or purchase from our site. If you are not a member then register now!";
            return RedirectToAction("Login","Home");
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
        [HttpGet]
        public ActionResult ContactWithSupportForUnblock()
        {
            if (CheckIfBlocked())
            {
                return View();
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult ContactWithSupportForUnblock(RequestToSupport request)
        {
            if (CheckIfBlocked())
            {
                if (ModelState.IsValid)
                {
                    request.SenderUserName = Session["UserName"].ToString();
                    request.UserType = Session["UserType"].ToString();
                    rtsrepo.Insert(request);
                    return View("Conformation");
                }
                else
                    TempData["RequestBody"] = request.RequestBody;
                    return View();
            }
            else
                return RedirectToAction("Login", "Home");
        }
    }
}