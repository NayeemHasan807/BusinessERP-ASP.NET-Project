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
    public class ProfileController : BaseController
    {
        private CustomerRepository customerrepo = new CustomerRepository();
        private VendorRepository vendorrepo = new VendorRepository();
        private EmployeeRepository employeerepo = new EmployeeRepository();
        private UserRepository userrepo = new UserRepository();
        private JobCategoryRepository jobcatrepo = new JobCategoryRepository();
        [HttpGet]
        public ActionResult Index()
        {
            if(Session["UserType"].ToString()=="Customer")
            {
                var profile = customerrepo.GetByUserName(Session["UserName"].ToString());
                var profileview = new ProfileViewModel();
                profileview.Id = profile.CustomerId;
                profileview.Name = profile.CustomerName;
                profileview.UserName = profile.UserName;
                profileview.Gender = profile.Gender;
                profileview.Email = profile.Email;
                profileview.DateOfBirth = profile.DateOfBirth;
                profileview.Address = profile.Address;
                profileview.ProfilePicture = profile.ProfilePicture;
                profileview.Status = profile.Status;
                return View(profileview);
            }
            else if(Session["UserType"].ToString() == "Vendor")
            {
                var profile = vendorrepo.GetByUserName(Session["UserName"].ToString());
                var profileview = new ProfileViewModel();
                profileview.Id = profile.VendorId;
                profileview.Name = profile.VendorName;
                profileview.UserName = profile.UserName;
                profileview.Gender = profile.Gender;
                profileview.Email = profile.Email;
                profileview.DateOfBirth = profile.DateOfBirth;
                profileview.Address = profile.Address;
                profileview.ProfilePicture = profile.ProfilePicture;
                profileview.Status = profile.Status;
                return View(profileview);
            }
            else
            {
                var profile = employeerepo.GetByUserName(Session["UserName"].ToString());
                TempData["JobDetails"] = jobcatrepo.GetById((int)profile.JobId);
                var profileview = new ProfileViewModel();
                profileview.Id = profile.EmployeeId;
                profileview.Name = profile.EmployeeName;
                profileview.UserName = profile.UserName;
                profileview.Gender = profile.Gender;
                profileview.Email = profile.Email;
                profileview.DateOfBirth = profile.DateOfBirth;
                profileview.Address = profile.Address;
                profileview.JoiningDate = profile.JoiningDate;
                profileview.ProfilePicture = profile.ProfilePicture;
                profileview.JobId = profile.JobId;
                profileview.Status = profile.Status;
                return View(profileview);
                
            }
        }
        [HttpGet]
        public ActionResult UpdateProfile()
        {
            if (Session["UserType"].ToString() == "Customer")
            {
                var profile = customerrepo.GetByUserName(Session["UserName"].ToString());
                var profileview = new ProfileViewModel();
                profileview.Id = profile.CustomerId;
                profileview.Name = profile.CustomerName;
                profileview.UserName = profile.UserName;
                profileview.Gender = profile.Gender;
                profileview.Email = profile.Email;
                profileview.DateOfBirth = profile.DateOfBirth;
                profileview.Address = profile.Address;
                profileview.ProfilePicture = profile.ProfilePicture;
                profileview.Status = profile.Status;
                return View(profileview);
            }
            else if (Session["UserType"].ToString() == "Vendor")
            {
                var profile = vendorrepo.GetByUserName(Session["UserName"].ToString());
                var profileview = new ProfileViewModel();
                profileview.Id = profile.VendorId;
                profileview.Name = profile.VendorName;
                profileview.UserName = profile.UserName;
                profileview.Gender = profile.Gender;
                profileview.Email = profile.Email;
                profileview.DateOfBirth = profile.DateOfBirth;
                profileview.Address = profile.Address;
                profileview.ProfilePicture = profile.ProfilePicture;
                profileview.Status = profile.Status;
                return View(profileview);
            }
            else
            {
                var profile = employeerepo.GetByUserName(Session["UserName"].ToString());
                TempData["JobCategory"] = jobcatrepo.GetAll();
                var profileview = new ProfileViewModel();
                profileview.Id = profile.EmployeeId;
                profileview.Name = profile.EmployeeName;
                profileview.UserName = profile.UserName;
                profileview.Gender = profile.Gender;
                profileview.Email = profile.Email;
                profileview.DateOfBirth = profile.DateOfBirth;
                profileview.Address = profile.Address;
                profileview.JoiningDate = profile.JoiningDate;
                profileview.ProfilePicture = profile.ProfilePicture;
                profileview.JobId = profile.JobId;
                profileview.Status = profile.Status;
                return View(profileview);

            }
        }
        [HttpPost]
        public ActionResult UpdateProfile(ProfileViewModel updatedata, HttpPostedFileBase image)
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

                        if (Session["UserType"].ToString() == "Customer")
                        {
                            var profile = customerrepo.GetByUserName(Session["UserName"].ToString());
                            profile.CustomerName = updatedata.Name;
                            profile.Email = updatedata.Email;
                            profile.Gender = updatedata.Gender;
                            profile.DateOfBirth = updatedata.DateOfBirth;
                            profile.Address = updatedata.Address;
                            profile.ProfilePicture = ProfilePicture;
                            customerrepo.Update(profile);
                            return RedirectToAction("Index");
                        }
                        else if (Session["UserType"].ToString() == "Vendor")
                        {
                            var profile = vendorrepo.GetByUserName(Session["UserName"].ToString());
                            profile.VendorName = updatedata.Name;
                            profile.Email = updatedata.Email;
                            profile.Gender = updatedata.Gender;
                            profile.DateOfBirth = updatedata.DateOfBirth;
                            profile.Address = updatedata.Address;
                            profile.ProfilePicture = ProfilePicture;
                            vendorrepo.Update(profile);
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            var profile = employeerepo.GetByUserName(Session["UserName"].ToString());
                            profile.EmployeeName = updatedata.Name;
                            profile.Email = updatedata.Email;
                            profile.Gender = updatedata.Gender;
                            profile.DateOfBirth = updatedata.DateOfBirth;
                            profile.Address = updatedata.Address;
                            profile.ProfilePicture = ProfilePicture;
                            employeerepo.Update(profile);
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Profile picture must need to be type '.jpg' or '.png'";
                        if (Session["UserType"].ToString() == "Customer")
                        {
                            var profile = customerrepo.GetByUserName(Session["UserName"].ToString());
                            var profileview = new ProfileViewModel();
                            profileview.Id = profile.CustomerId;
                            profileview.Name = profile.CustomerName;
                            profileview.UserName = profile.UserName;
                            profileview.Gender = profile.Gender;
                            profileview.Email = profile.Email;
                            profileview.DateOfBirth = profile.DateOfBirth;
                            profileview.Address = profile.Address;
                            profileview.ProfilePicture = profile.ProfilePicture;
                            profileview.Status = profile.Status;
                            return View(profileview);
                        }
                        else if (Session["UserType"].ToString() == "Vendor")
                        {
                            var profile = vendorrepo.GetByUserName(Session["UserName"].ToString());
                            var profileview = new ProfileViewModel();
                            profileview.Id = profile.VendorId;
                            profileview.Name = profile.VendorName;
                            profileview.UserName = profile.UserName;
                            profileview.Gender = profile.Gender;
                            profileview.Email = profile.Email;
                            profileview.DateOfBirth = profile.DateOfBirth;
                            profileview.Address = profile.Address;
                            profileview.ProfilePicture = profile.ProfilePicture;
                            profileview.Status = profile.Status;
                            return View(profileview);
                        }
                        else
                        {
                            var profile = employeerepo.GetByUserName(Session["UserName"].ToString());
                            TempData["JobCategory"] = jobcatrepo.GetAll();
                            var profileview = new ProfileViewModel();
                            profileview.Id = profile.EmployeeId;
                            profileview.Name = profile.EmployeeName;
                            profileview.UserName = profile.UserName;
                            profileview.Gender = profile.Gender;
                            profileview.Email = profile.Email;
                            profileview.DateOfBirth = profile.DateOfBirth;
                            profileview.Address = profile.Address;
                            profileview.JoiningDate = profile.JoiningDate;
                            profileview.ProfilePicture = profile.ProfilePicture;
                            profileview.JobId = profile.JobId;
                            profileview.Status = profile.Status;
                            return View(profileview);
                        }
                    }
                }
                else
                {
                    if (Session["UserType"].ToString() == "Customer")
                    {
                        var profile = customerrepo.GetByUserName(Session["UserName"].ToString());
                        profile.CustomerName = updatedata.Name;
                        profile.Email = updatedata.Email;
                        profile.Gender = updatedata.Gender;
                        profile.DateOfBirth = updatedata.DateOfBirth;
                        profile.Address = updatedata.Address;
                        customerrepo.Update(profile);
                        return RedirectToAction("Index");
                    }
                    else if (Session["UserType"].ToString() == "Vendor")
                    {
                        var profile = vendorrepo.GetByUserName(Session["UserName"].ToString());
                        profile.VendorName = updatedata.Name;
                        profile.Email = updatedata.Email;
                        profile.Gender = updatedata.Gender;
                        profile.DateOfBirth = updatedata.DateOfBirth;
                        profile.Address = updatedata.Address;
                        vendorrepo.Update(profile);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        var profile = employeerepo.GetByUserName(Session["UserName"].ToString());
                        profile.EmployeeName = updatedata.Name;
                        profile.Email = updatedata.Email;
                        profile.Gender = updatedata.Gender;
                        profile.DateOfBirth = updatedata.DateOfBirth;
                        profile.Address = updatedata.Address;
                        employeerepo.Update(profile);
                        return RedirectToAction("Index");
                    }
                }
            }
            else
            {
                if (Session["UserType"].ToString() == "Customer")
                {
                    var profile = customerrepo.GetByUserName(Session["UserName"].ToString());
                    var profileview = new ProfileViewModel();
                    profileview.Id = profile.CustomerId;
                    profileview.Name = profile.CustomerName;
                    profileview.UserName = profile.UserName;
                    profileview.Gender = profile.Gender;
                    profileview.Email = profile.Email;
                    profileview.DateOfBirth = profile.DateOfBirth;
                    profileview.Address = profile.Address;
                    profileview.ProfilePicture = profile.ProfilePicture;
                    profileview.Status = profile.Status;
                    return View(profileview);
                }
                else if (Session["UserType"].ToString() == "Vendor")
                {
                    var profile = vendorrepo.GetByUserName(Session["UserName"].ToString());
                    var profileview = new ProfileViewModel();
                    profileview.Id = profile.VendorId;
                    profileview.Name = profile.VendorName;
                    profileview.UserName = profile.UserName;
                    profileview.Gender = profile.Gender;
                    profileview.Email = profile.Email;
                    profileview.DateOfBirth = profile.DateOfBirth;
                    profileview.Address = profile.Address;
                    profileview.ProfilePicture = profile.ProfilePicture;
                    profileview.Status = profile.Status;
                    return View(profileview);
                }
                else
                {
                    var profile = employeerepo.GetByUserName(Session["UserName"].ToString());
                    TempData["JobCategory"] = jobcatrepo.GetAll();
                    var profileview = new ProfileViewModel();
                    profileview.Id = profile.EmployeeId;
                    profileview.Name = profile.EmployeeName;
                    profileview.UserName = profile.UserName;
                    profileview.Gender = profile.Gender;
                    profileview.Email = profile.Email;
                    profileview.DateOfBirth = profile.DateOfBirth;
                    profileview.Address = profile.Address;
                    profileview.JoiningDate = profile.JoiningDate;
                    profileview.ProfilePicture = profile.ProfilePicture;
                    profileview.JobId = profile.JobId;
                    profileview.Status = profile.Status;
                    return View(profileview);
                }
            }
        }
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel change)
        {
            if (ModelState.IsValid)
            {
                var user = userrepo.GetByUserName(Session["UserName"].ToString());
                if (change.Password == user.Password)
                {
                    if (change.NewPassword == change.ReNewPassword)
                    {
                        user.Password = change.NewPassword;
                        userrepo.Update(user);
                        TempData["ChangePassConf"] = "You password changed successfully!";
                        return RedirectToAction("Index");
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
    }
}