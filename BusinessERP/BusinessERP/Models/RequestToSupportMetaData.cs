using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BusinessERP.Models
{
    public class RequestToSupportMetaData
    {
        public int RequestId { get; set; }
        [Required, MinLength(5), MaxLength(100)]
        public string RequestSubject { get; set; }
        [Required,MinLength(10),MaxLength(500)]
        public string RequestBody { get; set; }
        public string SenderUserName { get; set; }
        public string UserType { get; set; }
    }
}