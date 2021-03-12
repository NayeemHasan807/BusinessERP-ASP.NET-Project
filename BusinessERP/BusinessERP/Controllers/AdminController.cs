using BusinessERP.Models.ViewModels;
using BusinessERP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessERP.Controllers
{
    public class AdminController : BaseController
    {
        private EmployeeRepository employeerepo = new EmployeeRepository();
        private JobCategoryRepository jobcatrepo = new JobCategoryRepository();
        private UserRepository userrepo = new UserRepository();
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
                TempData["Profile"] = employeerepo.GetByUserName(Session["UserName"].ToString());
                return View();
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult MyProfile()
        {
            if (CheckAccess())
            {
                var profile = employeerepo.GetByUserName(Session["UserName"].ToString());
                TempData["Profile"] = profile;
                TempData["JobDetails"] = jobcatrepo.GetById((int)profile.JobId);
                return View();
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel change)
        {
            if (ModelState.IsValid)
            {
                var user = userrepo.GetByUserName(Session["UserName"].ToString());
                if(change.Password==user.Password)
                {
                    if(change.NewPassword == change.ReNewPassword)
                    {
                        user.Password = change.NewPassword;
                        userrepo.Update(user);
                        return RedirectToAction("MyProfile","Admin");
                    }
                    else
                    {
                        TempData["NpAndRnp"] = "New password and retype new password must need to be same";
                        return View();

                    }
                }
                else
                {
                    TempData["OldP"] = "Old password is incorrect";
                    return View();
                }
            }
            else
                return View();
        }
    }
}