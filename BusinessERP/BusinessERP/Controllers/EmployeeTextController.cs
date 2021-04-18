using BusinessERP.Models;
using BusinessERP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessERP.Controllers
{
    public class EmployeeTextController : BaseController
    {
        private EmployeeRepository employeerepo = new EmployeeRepository();
        private EmployeeTextRepository textrepo = new EmployeeTextRepository();
        
        public bool CheckAccess()
        {
            var person = employeerepo.GetByUserName(Session["UserName"].ToString());
            if (person!=null)
            {
                return true;
            }
            else
                return false;
        }
        [HttpGet]
        public ActionResult Index()
        {
            if (CheckAccess())
            {
                return View(textrepo.GetAllReceivedByUserName(Session["UserName"].ToString()));
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult SendByMe()
        {
            if (CheckAccess())
            {
                return View(textrepo.GetAllSentByUserName(Session["UserName"].ToString()));
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (CheckAccess())
            {
                return View(textrepo.GetById(id));
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpPost,ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            if (CheckAccess())
            {
                textrepo.Delete(id);
                return RedirectToAction("SendByMe", "EmployeeText");
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult Create()
        {
            if (CheckAccess())
            {
                return View();
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult Create(EmployeeText employeeText)
        {
            if (CheckAccess())
            {
                if (ModelState.IsValid)
                {
                    var checkperson = employeerepo.GetByUserName(employeeText.ReceiverUserName);
                    if (checkperson != null)
                    {
                        textrepo.Insert(employeeText);
                        return RedirectToAction("SendByMe");
                    }
                    TempData["Error"] = "Their is no employee with this username";
                    TempData["Receiver"] = employeeText.ReceiverUserName;
                    TempData["TextBody"] = employeeText.TextBody;
                    return View(employeeText);
                }
                else
                {
                    TempData["Receiver"] = employeeText.ReceiverUserName;
                    TempData["TextBody"] = employeeText.TextBody;
                    return View(employeeText);
                }
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult Reply(string sun)
        {
            if (CheckAccess())
            {
                TempData["sun"] = sun;
                return View();
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult Reply(string sun,EmployeeText employeeText)
        {
            if (CheckAccess())
            {
                if (ModelState.IsValid)
                {
                    textrepo.Insert(employeeText);
                    return RedirectToAction("SendByMe");
                }
                else
                {
                    TempData["sun"] = sun;
                    TempData["TextBody"] = employeeText.TextBody;
                    return View(employeeText);
                }
            }
            else
                return RedirectToAction("Login", "Home");
        }
    }
}