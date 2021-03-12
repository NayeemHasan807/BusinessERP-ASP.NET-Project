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
    }
}