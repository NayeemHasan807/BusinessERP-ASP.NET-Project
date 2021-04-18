using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BusinessERP.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required,MinLength(4),Display(Name ="Old Password")]
        public string Password { get; set; }
        [Required,MinLength(4),Display(Name = "New Password")]
        public string NewPassword { get; set; }
        [Required,MinLength(4),Display(Name = "Retype New Password")]
        public string ReNewPassword { get; set; }
    }
}