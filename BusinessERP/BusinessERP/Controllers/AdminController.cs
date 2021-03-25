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
        private NoticeRepository noticerepo = new NoticeRepository();
        //Check access to admin actions
        public bool CheckAccess()
        {
            if (HttpContext.Session["UserType"].ToString() == "Admin")
                return true;
            else
                return false;
        }
        //admin dashboard
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
        
        //Admin actions on his/her access password
        [HttpGet]
        public ActionResult ChangePassword()
        {
            if (CheckAccess())
            {
                return View();
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel change)
        {
            if (CheckAccess())
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
            else
                return RedirectToAction("Login", "Home");
        }
        //Admin actions on his/her profile
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
        public ActionResult UpdateProfile()
        {
            if (CheckAccess())
            {
                var profile = employeerepo.GetByUserName(Session["UserName"].ToString());
                TempData["JobCategory"] = jobcatrepo.GetAll();
                return View(profile);
            }
            else
                return RedirectToAction("Login", "Home");

        }
        [HttpPost]
        public ActionResult UpdateProfile(Employee employee, HttpPostedFileBase image)
        {
            if (CheckAccess())
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

                        employeerepo.Update(profile);
                        return RedirectToAction("MyProfile","Admin");
                    }
                } 
                return View(profile);
            }
            else
                return RedirectToAction("Login", "Home");
        }
        //Admin actions on notice
        [HttpGet]
        public ActionResult CreateNewNotice()
        {
            if (CheckAccess())
            {
                return View();
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult CreateNewNotice(Notice notice)
        {
            if (CheckAccess())
            {
                if (ModelState.IsValid)
                {
                    noticerepo.Insert(notice);
                    return RedirectToAction("ViewAllNotice","Admin");
                }
                else
                {
                    TempData["NoticeTitle"] = notice.NoticeTitle;
                    TempData["NoticeBody"] = notice.NoticeBody;
                    return View();
                }
            }
            else
                return RedirectToAction("Login", "Home");

        }
        [HttpGet]
        public ActionResult ViewAllNotice()
        {
            if (CheckAccess())
            {
                return View(noticerepo.GetAll());
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult NoticeDetails(int id)
        {
            if (CheckAccess())
            {
                return View(noticerepo.GetById(id));
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult NoticeDelete(int id)
        {
            if (CheckAccess())
            {
                return View(noticerepo.GetById(id));
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpPost,ActionName("NoticeDelete")]
        public ActionResult ConfirmNoticeDelete(int id)
        {
            if (CheckAccess())
            {
                noticerepo.Delete(id);
                return RedirectToAction("ViewAllNotice", "Admin");
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult NoticeEdit(int id)
        {
            if (CheckAccess())
            {
                return View(noticerepo.GetById(id));
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult NoticeEdit(int id,Notice notice)
        {
            if (CheckAccess())
            {
                noticerepo.Update(notice);
                return RedirectToAction("ViewAllNotice","Admin");
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult Report()
        {
            if (CheckAccess())
            {
                return View();
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult UserReport()
        {
            if (CheckAccess())
            {
                return View();
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult UserBarChart()
        {
            if (CheckAccess())
            {
                var user = userrepo.GetAll();
                var category = user.Select(x => x.UserType).Distinct();
                List<int> value = new List<int>();
                foreach(var item in category)
                {
                    value.Add(user.Count(x => x.UserType == item));
                }
                return Json(new{ category, value }, JsonRequestBehavior.AllowGet);
            }
            else
                return RedirectToAction("Login", "Home");
        }

    }
}