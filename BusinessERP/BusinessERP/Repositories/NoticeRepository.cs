using BusinessERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessERP.Repositories
{
    public class NoticeRepository:Repository<Notice>
    {
        public List<Notice> GetAllForCustomer()
        {
            var noticesforcustomer = context.Notices.Where(x => x.ReceiverType == "Customer" || x.ReceiverType == "All").ToList();
            return noticesforcustomer;
        }
    }
}