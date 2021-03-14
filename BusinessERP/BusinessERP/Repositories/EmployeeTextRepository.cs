using BusinessERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessERP.Repositories
{
    public class EmployeeTextRepository:Repository<EmployeeText>
    {
        public List<EmployeeText> GetAllReceivedByUserName(string username)
        {
            return context.EmployeeTexts.Where(x=> x.ReceiverUserName == username).ToList();
        }
        public List<EmployeeText> GetAllSentByUserName(string username)
        {
            return context.EmployeeTexts.Where(x => x.SenderUserName == username).ToList();
        }
    }
}