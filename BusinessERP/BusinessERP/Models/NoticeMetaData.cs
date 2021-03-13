using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BusinessERP.Models
{
    public class NoticeMetaData
    {
        public int NoticeId { get; set; }
        [Required,MinLength(10),MaxLength(30)]
        public string NoticeTitle { get; set; }
        [Required,MinLength(20),MaxLength(200)]
        public string NoticeBody { get; set; }
        [Required]
        public string ReceiverType { get; set; }
    }
}