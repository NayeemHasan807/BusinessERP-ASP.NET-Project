using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BusinessERP.Models
{
    public class EmployeeMetaData
    {
        public int EmployeeId { get; set; }
        [Required,MinLength(4),MaxLength(30)]
        public string EmployeeName { get; set; }
        [Required,MinLength(3),Display(Name ="Full Name")]
        public string UserName { get; set; }
        [Required, EmailAddress] 
        public string Email { get; set; }
        [Required] 
        public string Gender { get; set; }
        [Required] 
        public System.DateTime DateOfBirth { get; set; }
        [Required] 
        public string Address { get; set; }
        [Required] 
        public System.DateTime JoiningDate { get; set; }
        public string ProfilePicture { get; set; }
        [Required] 
        public string Status { get; set; }
        [Required] 
        public int JobId { get; set; }
    }
}