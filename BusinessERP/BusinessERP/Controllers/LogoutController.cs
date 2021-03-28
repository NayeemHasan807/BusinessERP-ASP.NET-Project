using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessERP.Controllers
{
    public class LogoutController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}