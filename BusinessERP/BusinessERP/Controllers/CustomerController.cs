using BusinessERP.Models;
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
    }
}