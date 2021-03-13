using BusinessERP.Models;
using BusinessERP.Models.ViewModels;
using BusinessERP.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
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
        [HttpGet]
        public ActionResult UpdateProfile()
        {
            var profile = employeerepo.GetByUserName(Session["UserName"].ToString());
            TempData["JobCategory"] = jobcatrepo.GetAll();
            return View(profile);
        }
        [HttpPost]
        public ActionResult UpdateProfile(Employee employee, HttpPostedFileBase image)
        {
            var profile = employeerepo.GetByUserName(Session["UserName"].ToString());
            TempData["JobCategory"] = jobcatrepo.GetAll();
            if (ModelState.IsValid)
            {
                if(image != null)
                {
                    if (Path.GetExtension(image.FileName)==".jpg" | Path.GetExtension(image.FileName) == ".png")
                    {
                        string name = Path.GetFileNameWithoutExtension(image.FileName);
                        string extention = Path.GetExtension(image.FileName);
                        name = name + DateTime.Now.ToString("yyyy-MM-dd")+extention;
                        string ProfilePicture = "~/Content/ProfilePictures/" + name;
                        string filepath = Path.Combine(Server.MapPath("~/Content/ProfilePictures/"),name);
                        image.SaveAs(filepath);

                        profile.EmployeeName = employee.EmployeeName;
                        profile.Email = employee.Email;
                        profile.Gender = employee.Gender;
                        profile.DateOfBirth = employee.DateOfBirth;
                        profile.Address = employee.Address;
                        profile.JoiningDate = employee.JoiningDate;
                        profile.JobId = employee.JobId;
                        profile.ProfilePicture = ProfilePicture;

                        employeerepo.Update(profile);
                        return RedirectToAction("MyProfile", "Admin");
                    }
                    else
                    {
                        TempData["Error"] = "Profile picture must need to be type '.jpg' or '.png'";
                        return View(profile);
                    }
                }
                else
                {
                    profile.EmployeeName = employee.EmployeeName;
                    profile.Email = employee.Email;
                    profile.Gender = employee.Gender;
                    profile.DateOfBirth = employee.DateOfBirth;
                    profile.Address = employee.Address;
                    profile.JoiningDate = employee.JoiningDate;
                    profile.JobId = employee.JobId;

                    employeerepo.Update(profile);
                    return RedirectToAction("MyProfile","Admin");
                }
            } 
            return View(profile);
        }
    }
}