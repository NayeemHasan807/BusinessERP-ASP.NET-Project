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
        private CompanyProductRepository comprodrepo = new CompanyProductRepository();
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
                ViewData["StockOut"] = comprodrepo.StockOut();
                ViewData["LowStock"] = comprodrepo.LowStock();
                TempData["Profile"] = employeerepo.GetByUserName(Session["UserName"].ToString());
                return View();
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
        //Admin Report
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