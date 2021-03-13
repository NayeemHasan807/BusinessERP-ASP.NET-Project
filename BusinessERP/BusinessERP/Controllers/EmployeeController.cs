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
    public class EmployeeController : BaseController
    {
        private EmployeeRepository employeerepo = new EmployeeRepository();
        private UserRepository userrepo = new UserRepository();
        private JobCategoryRepository jobcatrepo = new JobCategoryRepository();
        public bool CheckAccess()
        {
            if (Session["UserType"].ToString() == "Admin")
            {
                return true;
            }
            else
                return false;
        }

        [HttpGet]
        public ActionResult Index()
        {
            if(CheckAccess())
            {
                return View(employeerepo.GetAll());
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult Create()
        {
            if (CheckAccess())
            {
                TempData["JobCategory"] = jobcatrepo.GetAll();
                return View();
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult Create(Employee employee,HttpPostedFileBase image)
        {
            if (CheckAccess())
            {
                if (ModelState.IsValid)
                {
                    var person = userrepo.GetByUserName(employee.UserName);
                    if(person == null)
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
                                employee.ProfilePicture = ProfilePicture;
                                User user = new User();
                                JobCategory job = jobcatrepo.GetById(employee.JobId);
                                user.UserName = employee.UserName;
                                user.Password = employee.UserName;
                                user.UserType = job.JobTitle;
                                user.UserStatus = employee.Status;
                                employeerepo.Insert(employee);
                                userrepo.Insert(user);
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                TempData["Error2"] = "Profile picture must need to be type '.jpg' or '.png'";
                                TempData["JobCategory"] = jobcatrepo.GetAll();
                                return View(employee);
                            }
                        }
                        TempData["Error2"] = "Must need to add a profile picture";
                        TempData["JobCategory"] = jobcatrepo.GetAll();
                        return View(employee);
                    }
                    TempData["Error1"] = "This username is taken";
                    TempData["JobCategory"] = jobcatrepo.GetAll();
                    return View(employee);

                }
                TempData["JobCategory"] = jobcatrepo.GetAll();
                return View(employee);
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            if (CheckAccess())
            {
                return View(employeerepo.GetById(id));
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (CheckAccess())
            {
                return View(employeerepo.GetById(id));
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpPost,ActionName("Delete")]
        public ActionResult ConfirmDelete(int id, string username)
        {
            if (CheckAccess())
            {
                employeerepo.Delete(id);
                userrepo.DeleteByUsername(username);
                return RedirectToAction("Index", "Employee");
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (CheckAccess())
            {
                TempData["JobCategory"] = jobcatrepo.GetAll();
                return View(employeerepo.GetById(id));
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult Edit(int id,Employee employee,HttpPostedFileBase image)
        {
            if (CheckAccess())
            {
                if (ModelState.IsValid)
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

                            employee.ProfilePicture = ProfilePicture;

                            employeerepo.Update(employee);
                            return RedirectToAction("Index", "Employee");
                        }
                        else
                        {
                            TempData["Error"] = "Profile picture must need to be type '.jpg' or '.png'";
                            TempData["JobCategory"] = jobcatrepo.GetAll();
                            return View(employeerepo.GetById(id));
                        }
                    }
                    else
                    {
                        employeerepo.Update(employee);
                        return RedirectToAction("Index", "Employee");
                    }
                }
                TempData["JobCategory"] = jobcatrepo.GetAll();
                return View(employeerepo.GetById(id));
            }
            else
                return RedirectToAction("Login", "Home");
        }
    }
}