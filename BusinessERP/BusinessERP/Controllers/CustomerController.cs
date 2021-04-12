using BusinessERP.Models;
using BusinessERP.Models.ViewModels;
using BusinessERP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessERP.Controllers
{
    public class CustomerController : BaseController
    {
        private CompanyProductRepository comprodrepo = new CompanyProductRepository();
        private CustomerRepository customerrepo = new CustomerRepository();
        private NoticeRepository noticerepo = new NoticeRepository();
        private CustomerInvoiceRepository cusinvrepo = new CustomerInvoiceRepository();
        private CustomerLineItemRepository cuslirepo = new CustomerLineItemRepository();
        private RequestToSupportRepository rtsrepo = new RequestToSupportRepository();
        //Check function access of customer
        public bool CheckIfCustomer()
        {
            if (HttpContext.Session["UserType"].ToString() == "Customer")
                return true;
            else
                return false;
        }
        //Customer Home
        [HttpGet]
        public ActionResult Index()
        {
            if (CheckIfCustomer())
            {
                var customer = customerrepo.GetByUserName(Session["UserName"].ToString());
                return View(customer);
            }
            else
                return RedirectToAction("Login", "Home");
        }
        //Cart for customer
        [HttpPost]
        public ActionResult AddToCart(CompanyProduct selected)
        {
            if (CheckIfCustomer())
            {
                if (ModelState.IsValid)
                {
                    CompanyProduct product = comprodrepo.GetById(selected.ProductId);
                    if (product.Quantity >= selected.Quantity)
                    {
                        var inCart = customerrepo.GetProductFromCart(selected.ProductId);
                        if (inCart == null)
                        {
                            product.Quantity = selected.Quantity;
                            customerrepo.AddToCart(product);
                            return RedirectToAction("Index", "CompanyProduct");
                        }
                        else
                        {
                            var count = inCart.Quantity + selected.Quantity;
                            if (product.Quantity>= count)
                            {
                                product.Quantity = selected.Quantity;
                                customerrepo.AddToCart(product);
                                return RedirectToAction("Index", "CompanyProduct");
                            }
                            else
                            {
                                var canget = product.Quantity - inCart.Quantity;
                                TempData["Error"] = "Your previous cart quantiry and recent cart quantity are extending available quantity. You can add max "+canget+ " Of this product to your cart";
                                return RedirectToAction("ViewProduct", "CompanyProduct", new { id = selected.ProductId });
                            }
                        }
                        
                    }
                    TempData["Error"] = "Quantity must be less then or equals to available quantity";
                    return RedirectToAction("ViewProduct", "CompanyProduct", new { id = selected.ProductId });
                }
                else
                {
                    TempData["Error"] = "Can't be empty and Quentity must be in between 1 to available quantities";
                    return RedirectToAction("ViewProduct", "CompanyProduct", new { id = selected.ProductId });
                }
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult ViewCart()
        {
            if (CheckIfCustomer())
            {
                return View(customerrepo.ViewCart());
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult RemoveFromCart(int id)
        {
            if (CheckIfCustomer())
            {
                customerrepo.RemoveFromCart(id);
                return RedirectToAction("ViewCart","Customer");
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult EditQuantity(int id)
        {
            if (CheckIfCustomer())
            {
                ViewBag.AQuantity = (int)comprodrepo.GetById(id).Quantity;
                ViewBag.RQuantity = (int)customerrepo.GetProductFromCart(id).Quantity;
                return View(customerrepo.GetProductFromCart(id));
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult EditQuantity(CompanyProduct product)
        {
            if (CheckIfCustomer())
            {
                if (ModelState.IsValid)
                {
                    ViewBag.AQuantity = (int)comprodrepo.GetById(product.ProductId).Quantity;
                    if (ViewBag.AQuantity >= product.Quantity)
                    {
                        customerrepo.EditQuantity(product);
                        return RedirectToAction("ViewCart", "Customer");
                    }
                    else
                    {
                        TempData["Error"] = "Quantity must be less then or equals to available quantity";
                        return RedirectToAction("EditQuantity", "Customer", new { id = product.ProductId });
                    }
                }
                else
                {
                    TempData["Error"] = "Can't be empty and Quentity must be in between 1 to available quantities";
                    return RedirectToAction("EditQuantity", "Customer", new { id = product.ProductId });
                }
            }
            else
                return RedirectToAction("Login", "Home");
        }
        //Customer Checkout
        [HttpGet]
        public ActionResult Checkout()
        {
            if (CheckIfCustomer())
            {
                TempData["Checkout"] = customerrepo.CheckoutDetails();
                return View();
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult Checkout(CustomerInvoice invoice)
        {
            if (CheckIfCustomer())
            {
                if(ModelState.IsValid)
                {
                    CheckoutViewModel checkoutDetails = customerrepo.CheckoutDetails();
                    invoice.SubTotal = checkoutDetails.TotalPrice;
                    invoice.TotalWithTax = checkoutDetails.TotalPriceWithTax;
                    invoice.CustomerUserName = Session["UserName"].ToString();
                    cusinvrepo.Insert(invoice);
                    CustomerInvoice lastinvoice=null; 
                    foreach(var item in cusinvrepo.GetAll())
                    {
                        lastinvoice = item;
                    }
                    foreach(var item in checkoutDetails.CartProductList)
                    {
                        CustomerLineItem lineItem = new CustomerLineItem();
                        lineItem.InvoiceId = lastinvoice.InvoiceId;
                        lineItem.ProductId = item.ProductId;
                        lineItem.Quantity = item.Quantity;
                        lineItem.UnitPrice = item.UnitPrice;
                        lineItem.Total = item.Quantity * item.UnitPrice;
                        cuslirepo.Insert(lineItem);
                        CompanyProduct product = comprodrepo.GetById(item.ProductId);
                        product.Quantity = product.Quantity - item.Quantity;
                        comprodrepo.Update(product);
                    }
                    customerrepo.ClearCart();
                    return View("Conformation");
                   
                }
                else
                {
                    TempData["Checkout"] = customerrepo.CheckoutDetails();
                    return View(invoice);
                }
            }
            else
                return RedirectToAction("Login", "Home");
        }
        //Customer notice check
        [HttpGet]
        public ActionResult ViewNotice()
        {
            if (CheckIfCustomer())
            {
                return View(noticerepo.GetAllForCustomer());
            }
            else
                return RedirectToAction("Login", "Home");
        }
        //Customer request for support
        [HttpGet]
        public ActionResult SupportRequest()
        {
            if (CheckIfCustomer())
            {
                return View();
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult SupportRequest(RequestToSupport request)
        {
            if (CheckIfCustomer())
            {
                if (ModelState.IsValid)
                {
                    request.SenderUserName = Session["UserName"].ToString();
                    request.UserType = Session["UserType"].ToString();
                    rtsrepo.Insert(request);
                    TempData["Conformation"]="Your request is send to support. We will contact with you via mail within 24hours";
                    return RedirectToAction("Index");
                }
                else
                    return View(request);
            }
            else
                return RedirectToAction("Login", "Home");
        }
        //Customer reports
        [HttpGet]
        public ActionResult Report()
        {
            if (CheckIfCustomer())
            {
                return View();
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult ProductPerchesReport()
        {
            if (CheckIfCustomer())
            {
                return View();
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult ShoppingExpensesReport()
        {
            if (CheckIfCustomer())
            {
                return View();
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult ProductPerchesBarChart()
        {
            if (CheckIfCustomer())
            {
                var customerinvoice = cusinvrepo.GetAllByUserName(Session["UserName"].ToString());
                List<ReportViewModel> view = new List<ReportViewModel>();
                foreach (var item in customerinvoice)
                {
                    var lineItems = cuslirepo.GetByInvoiceId(item.InvoiceId);
                    foreach(var items in lineItems)
                    {
                        var indevisualproduct = comprodrepo.GetById(items.ProductId);
                        bool check = false;
                        foreach(var item_s in view)
                        {
                            if (item_s.Product.ProductId == indevisualproduct.ProductId)
                            {
                                item_s.Count = item_s.Count + items.Quantity;
                                check = true;
                                break;
                            }
                            else
                                continue;
                        }
                        if(check==false)
                        {
                            ReportViewModel newp = new ReportViewModel();
                            newp.Product = indevisualproduct;
                            newp.Count = items.Quantity;
                            view.Add(newp);
                        }
                    }
                }
                List<string> ProductName = new List<string>();
                List<int> PerchesQuantity = new List<int>();
                foreach(var item in view)
                {
                    ProductName.Add(item.Product.ProductName);
                    PerchesQuantity.Add(item.Count);
                }
                return Json(new { ProductName , PerchesQuantity }, JsonRequestBehavior.AllowGet);
            }
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult ShoppingExpensesLineChart()
        {
            if (CheckIfCustomer())
            {
                var customerinvoice = cusinvrepo.GetAllByUserName(Session["UserName"].ToString());
                List<string> date = new List<string>();
                var fetchdate = customerinvoice.Select(x => x.OrderDate).Distinct().OrderBy(y => y.Date);
                foreach (var item in fetchdate)
                {
                    var i = item.ToString("dd-MM-yyyy");
                    date.Add(i);
                }
                List<double> sales = new List<double>();
                foreach (var item in fetchdate)
                {
                    var info = customerinvoice.Where(x => x.OrderDate == item).ToList();
                    double count = 0;
                    foreach (var i in info)
                    {
                        count = count + i.TotalWithTax;
                    }
                    sales.Add(Math.Round(count));
                }
                return Json(new { date, sales }, JsonRequestBehavior.AllowGet);
            }
            else
                return RedirectToAction("Login", "Home");
        }
    }
}