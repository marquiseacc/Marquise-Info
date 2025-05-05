using System;
using System.Collections.Generic;
using System.Security.Claims;
using Utilities.Convert;

namespace Marquise_Web.UI.areas.CRM.Models
{
    public class StaffInfo
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

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
                if (Status == "b1af65c3-af6e-48a6-8d1f-b5c09d7f03c9")
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

    public class TicketDetailVm
    {
        public string TicketId { get; set; }
        public string TicketNumber { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateDatePersian => CreateDate.ToPersianDateString();
        public string ITStaffId { get; set; }
        public StaffInfo Staff { get; set; }
        public List<ShowAnswerVM> Answers { get; set; }
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

    public class ShowAnswerVM
    {
        public string CreateBy { get; set; }
        public StaffInfo Staff { get; set; }
        public string StaffName { get; set; }
        public DateTime CreateDate { get; set; }
        public string Body { get; set; }
    }

    public class AnswerVM
    {
        public string TicketId { get; set; }
        public string Message { get; set; }
        
    }

    public class NewTicketVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class RootResponse
    {
        public bool Succeeded { get; set; }
        public string ResultId { get; set; }
        public TicketResultData ResultData { get; set; }
    }

    public class TicketResultData
    {
        public string rowId { get; set; }
        public object JTRowId { get; set; }
        public List<IncrementalField> IncrementalFields { get; set; }
    }

    public class IncrementalField
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class CloseTicket
    {
        public string TicketId { get; set; }
    }
}