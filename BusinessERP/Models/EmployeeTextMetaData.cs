using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BusinessERP.Models
{
    public class EmployeeTextMetaData
    {
        public int TextId { get; set; }
        [Required,MaxLength(500),Display(Name = "Text")]
        public string TextBody { get; set; }
        [Required,Display(Name ="Receiver")]
        public string ReceiverUserName { get; set; }
        [Required]
        public string SenderUserName { get; set; }
    }
}