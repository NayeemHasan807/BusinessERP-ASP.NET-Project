using BusinessERP.Models;
using BusinessERP.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessERP.Repositories
{
    public class CustomerRepository : Repository<Customer>
    {
        public Customer GetByUserName(string username)
        {
            return context.Customers.Where(x => x.UserName == username).FirstOrDefault();
        }
        public List<Customer> GetBySearch(string searchkey, string status)
        {
            if (searchkey != null)
            {
                if (status != "All")
                {
                    var list1 = context.Customers.Where(x => x.CustomerName.Contains(searchkey)).Where(x => x.Status == status).ToList();
                    return list1;
                }
                else
                {
                    var list1 = context.Customers.Where(x => x.CustomerName.Contains(searchkey)).ToList();
                    return list1;
                }

            }
            else
            {
                if (status != "All")
                {
                    var list2 = context.Customers.Where(x => x.Status == status).ToList();
                    return list2;
                }
                else
                {
                    var list2 = context.Customers.ToList();
                    return list2;
                }
            }
        }
        public void AddToCart(CompanyProduct product)
        {
            List<CompanyProduct> cart;
            object objCart = HttpContext.Current.Session["cart"];
            cart = objCart as List<CompanyProduct>;
            if (cart == null)
            {
                cart = new List<CompanyProduct>();
                HttpContext.Current.Session["cart"] = cart;
            }
            if (product.ProductId.ToString() != null)
            {
                var pID = product.ProductId;
                var inCart = cart.Where(x => x.ProductId == pID).FirstOrDefault();
                if (inCart == null)
                {
                    cart.Add(product);
                    HttpContext.Current.Session["cart"] = cart;
                }
                else
                {
                    cart.Where(x => x.ProductId == pID).FirstOrDefault().Quantity = cart.Where(x => x.ProductId == pID).FirstOrDefault().Quantity + product.Quantity;
                    HttpContext.Current.Session["cart"] = cart;
                }
            }
        }
        public CompanyProduct GetProductFromCart(int id)
        {
            List<CompanyProduct> cart;
            object objCart = HttpContext.Current.Session["cart"];
            cart = objCart as List<CompanyProduct>;
            if(cart!=null)
            {
                var inCart = cart.Where(x => x.ProductId == id).FirstOrDefault();
                return inCart;
            }
            return null;
        }
        public List<CompanyProduct> ViewCart()
        {
            List<CompanyProduct> cart;
            object objCart = HttpContext.Current.Session["cart"];
            cart = objCart as List<CompanyProduct>;
            return cart;
        }

        public void RemoveFromCart(int id)
        {
            List<CompanyProduct> cart;
            object objCart = HttpContext.Current.Session["cart"];
            cart = objCart as List<CompanyProduct>;
            cart.Remove(cart.Where(x=>x.ProductId==id).FirstOrDefault());
            HttpContext.Current.Session["cart"]=cart;
        }
        public void EditQuantity(CompanyProduct product)
        {
            List<CompanyProduct> cart;
            object objCart = HttpContext.Current.Session["cart"];
            cart = objCart as List<CompanyProduct>;
            cart.Where(x => x.ProductId == product.ProductId).FirstOrDefault().Quantity=product.Quantity;
            HttpContext.Current.Session["cart"] = cart;
        }
        public CheckoutViewModel CheckoutDetails()
        {
            CheckoutViewModel details = new CheckoutViewModel();
            object objCart = HttpContext.Current.Session["cart"];
            details.CartProductList = objCart as List<CompanyProduct>;
            if (details.CartProductList != null)
            {
                foreach (var item in details.CartProductList)
                {
                    var pfori = item.Quantity * item.UnitPrice;
                    details.TotalPrice = details.TotalPrice + pfori;
                }
                details.TotalPriceWithTax = ((details.TotalPrice * 10) / 100) + details.TotalPrice;
                return details;
            }
            return null;
        }
        public void ClearCart()
        {
            HttpContext.Current.Session["cart"]=null;
        }
    }
}