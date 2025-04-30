using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilities.Convert;

namespace Marquise_Web.UI.areas.CRM.Models
{
    public class TicketVM
    {
        public string TicketId { get; set; }
        public string TicketNumber { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateDatePersian => CreateDate.ToPersianDateString();
        public string ITStaffId { get; set; }
        public StaffInfo Staff { get; set; }
        public string StaffName 
        { 
            get
            {
                return Staff.FirstName + " " + Staff.LastName;
            }
        }
        public string Status { get; set; }
        public string StatusTitle
        {
            get
            {
                if (Status == "3ab6b173-2e12-4e3f-813b-8a14ae01385d")
                    return "باز";
                else if (Status == "8804f420-0c59-44d2-a4ca-711af8822c56")
                    return "انجام شده";
                else if (Status == "9a5e80a8-cc75-46f1-b158-01d58384d4f7")
                    return "بسته";
                else
                    return "نامشخص";
                //else if (Status == "")
                //    return "منتظر پاسخ";
                //if (Status == "")
                //    return "جدید";
                /* else*/
            }
        }
    }

    public class StaffInfo
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class TicketDetailVm
    {
        public string TicketId { get; set; }
        public string TicketNumber { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateDatePersian => CreateDate.ToPersianDateString();
        public string ITStaffId { get; set; }
        public StaffInfo Staff { get; set; }
        public string StaffName
        {
            get
            {
                return Staff.FirstName + " " + Staff.LastName;
            }
        }
        public string Status { get; set; }
        public string StatusTitle
        {
            get
            {
                if (Status == "3ab6b173-2e12-4e3f-813b-8a14ae01385d")
                    return "باز";
                else if (Status == "8804f420-0c59-44d2-a4ca-711af8822c56")
                    return "انجام شده";
                else if (Status == "9a5e80a8-cc75-46f1-b158-01d58384d4f7")
                    return "بسته";
                else
                    return "نامشخص";
                //else if (Status == "")
                //    return "منتظر پاسخ";
                //if (Status == "")
                //    return "جدید";
                /* else*/
            }
        }
    }
}