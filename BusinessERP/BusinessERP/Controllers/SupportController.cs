using BusinessERP.Models;
using BusinessERP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessERP.Controllers
{
    public class SupportController : BaseController
    {
        private EmployeeRepository employeerepo = new EmployeeRepository();
        private RegistrationRequestRepository rrrepo = new RegistrationRequestRepository();
        private RequestToSupportRepository rtsrepo = new RequestToSupportRepository();
        private UserRepository userrepo = new UserRepository();
        private CustomerRepository customerrepo = new CustomerRepository();
        private VendorRepository vendorrepo = new VendorRepository();
        private RegistrationRequestLogRepository rrlogrepo = new RegistrationRequestLogRepository();
        private SupportLogRepository suplogrepo = new SupportLogRepository();
        
        //Support access check
        public bool CheckAccess()
        {
            if (HttpContext.Session["UserType"].ToString() == "Support")
                return true;
            else
                return false;
        }

        //Support home
        [HttpGet]
        public ActionResult Index()
        {
            if (CheckAccess())
            {
                ViewData["TotalRR"] = rrrepo.GetAll().Count();
                ViewData["TotalRFS"] = rtsrepo.GetAll().Count();
                return View(employeerepo.GetByUserName(Session["UserName"].ToString()));
            }
            else
                return RedirectToAction("Login", "Home");
        }

        //Support request list and controll for customer and vendor
        [HttpGet]
        public ActionResult ViewSupportRequest()
        {
            if (CheckAccess())
            {
                return View(rtsrepo.GetAll());
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult MakeAsChecked(int id)
        {
            var data = rtsrepo.GetById(id);
            SupportLog supportlog = new SupportLog();
            supportlog.RequestSubject = data.RequestSubject;
            supportlog.RequestBody = data.RequestBody;
            supportlog.SenderUserName = data.SenderUserName;
            supportlog.UserType = data.UserType;
            supportlog.Date = DateTime.Now;
            supportlog.SupportUserName = Session["UserName"].ToString();
            suplogrepo.Insert(supportlog);
            rtsrepo.Delete(id);
            return RedirectToAction("ViewSupportRequest");
        }

        //Customer list, details, block & unblock
        [HttpGet]
        public ActionResult CustomerList()
        {
            if (CheckAccess())
            {
                return View(customerrepo.GetAll());
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult CustomerDetails(int id)
        {
            if (CheckAccess())
            {
                return View(customerrepo.GetById(id));
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult CustomerBlock(int id)
        {
            if (CheckAccess())
            {
                var customerdata = customerrepo.GetById(id);
                var userdata = userrepo.GetByUserName(customerdata.UserName);
                customerdata.Status = "Blocked";
                userdata.UserStatus = "Blocked";
                customerrepo.Update(customerdata);
                userrepo.Update(userdata);
                return RedirectToAction("CustomerList");

            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult CustomerUnblock(int id)
        {
            if (CheckAccess())
            {
                var customerdata = customerrepo.GetById(id);
                var userdata = userrepo.GetByUserName(customerdata.UserName);
                customerdata.Status = "Active";
                userdata.UserStatus = "Active";
                customerrepo.Update(customerdata);
                userrepo.Update(userdata);
                return RedirectToAction("CustomerList");

            }
            else
                return RedirectToAction("Login", "Home");
        }

        //Vendor list, details, block & unblock
        [HttpGet]
        public ActionResult VendorList()
        {
            if (CheckAccess())
            {
                return View(vendorrepo.GetAll());
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult VendorDetails(int id)
        {
            if (CheckAccess())
            {
                return View(vendorrepo.GetById(id));
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult VendorBlock(int id)
        {
            if (CheckAccess())
            {
                var vendordata = vendorrepo.GetById(id);
                var userdata = userrepo.GetByUserName(vendordata.UserName);
                vendordata.Status = "Blocked";
                userdata.UserStatus = "Blocked";
                vendorrepo.Update(vendordata);
                userrepo.Update(userdata);
                return RedirectToAction("VendorList");

            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult VendorUnblock(int id)
        {
            if (CheckAccess())
            {
                var vendordata = vendorrepo.GetById(id);
                var userdata = userrepo.GetByUserName(vendordata.UserName);
                vendordata.Status = "Active";
                userdata.UserStatus = "Active";
                vendorrepo.Update(vendordata);
                userrepo.Update(userdata);
                return RedirectToAction("VendorList");

            }
            else
                return RedirectToAction("Login", "Home");
        }

        //Registration request list, details, approve & reject for customer and vendor
        [HttpGet]
        public ActionResult ViewRegistrationRequest()
        {
            if (CheckAccess())
            {
                return View(rrrepo.GetAll());
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult RRDetails(int id)
        {
            if (CheckAccess())
            {
                return View(rrrepo.GetById(id));
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult RRApprove(int id)
        {
            if (CheckAccess())
            {
                var request = rrrepo.GetById(id);
                
                var rrlogdata = new RegistrationRequestLog();
                rrlogdata.UserName = request.UserName;
                rrlogdata.UserType = request.UserType;
                rrlogdata.SupportUserName = Session["UserName"].ToString();
                rrlogdata.Date = DateTime.Now;
                rrlogdata.Status = "Accepted";

                var userdata = new User();
                userdata.UserName = request.UserName;
                userdata.Password = request.Password;
                userdata.UserType = request.UserType;
                userdata.UserStatus = "Active";
                
                var customerdata = new Customer();
                customerdata.CustomerName = request.Name;
                customerdata.UserName = request.UserName;
                customerdata.Email = request.Email;
                customerdata.Gender = request.Gender;
                customerdata.DateOfBirth = request.DateOfBirth;
                customerdata.Address = request.Address;
                customerdata.ProfilePicture = request.ProfilePicture;
                customerdata.Status = "Active";
                

                userrepo.Insert(userdata);
                customerrepo.Insert(customerdata);
                rrlogrepo.Insert(rrlogdata);
                rrrepo.Delete(id);
                return RedirectToAction("ViewRegistrationRequest");
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult RRReject(int id)
        {
            if (CheckAccess())
            {
                var request = rrrepo.GetById(id);
                var rrlogdata = new RegistrationRequestLog();
                rrlogdata.UserName = request.UserName;
                rrlogdata.UserType = request.UserType;
                rrlogdata.SupportUserName = Session["UserName"].ToString();
                rrlogdata.Date = DateTime.Now;
                rrlogdata.Status = "Rejected";
                rrlogrepo.Insert(rrlogdata);
                rrrepo.Delete(id);
                return RedirectToAction("ViewRegistrationRequest");
            }
            else
                return RedirectToAction("Login", "Home");
        }

        //Support Reports
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
        public ActionResult RegistrationLogReport()
        {
            if (CheckAccess())
            {
                return View();
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult RegistrationLogBarChart()
        {
            if (CheckAccess())
            {
                var log = rrlogrepo.GetAll();
                var category = log.Select(x => x.UserType).Distinct();
                List<int> acceptedvalue = new List<int>();
                List<int> rejectedvalue = new List<int>();
                foreach (var item in category)
                {
                    acceptedvalue.Add(log.Where(c=>c.Status=="Accepted").Where(c => c.SupportUserName == Session["UserName"].ToString()).Count(x => x.UserType == item));
                    rejectedvalue.Add(log.Where(c => c.Status == "Rejected").Where(c => c.SupportUserName == Session["UserName"].ToString()).Count(x => x.UserType == item));
                }
                return Json(new { category, acceptedvalue , rejectedvalue }, JsonRequestBehavior.AllowGet);
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult SupportLogReport()
        {
            if (CheckAccess())
            {
                return View();
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult SupportLogPieChart()
        {
            if (CheckAccess())
            {
                var log = suplogrepo.GetAll();
                var category = log.Select(x => x.UserType).Distinct();
                List<int> value = new List<int>();
                foreach (var item in category)
                {
                    value.Add(log.Where(c=>c.SupportUserName==Session["UserName"].ToString()).Count(x => x.UserType == item));
                }
                return Json(new { category, value}, JsonRequestBehavior.AllowGet);
            }
            else
                return RedirectToAction("Login", "Home");
        }
    }
}