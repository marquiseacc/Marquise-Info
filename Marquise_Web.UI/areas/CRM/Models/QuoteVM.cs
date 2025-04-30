using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilities.Convert;

namespace Marquise_Web.UI.areas.CRM.Models
{
    public class QuoteVM
    {
        public string QuoteId { get; set; }
        public int QuoteCode { get; set; }
        public string Name { get; set; }
        public double TotalPrice { get; set; }
        public double FinalAmount { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateDatePersian => CreateDate.ToPersianDateString();
        public string Status { get; set; }
        public string StatusTitle
        {
            get
            {
                if (Status == "5da43111-369a-4f7a-a024-6e470af1122f")
                    return "جدید";
                else if (Status == "ed9d3244-bc9f-4d67-8a24-5f580e951400")
                    return "تبدیل شده";
                else
                    return "نامشخص";
            }
        }
    }

    
    public class QuoteDetailVm
    {
        public string QuoteId { get; set; }
        public int QuoteCode { get; set; }
        public string Name { get; set; }
        public double TotalPrice { get; set; }
        public double FinalAmount { get; set; }
        public double TotalDiscount { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateDatePersian => CreateDate.ToPersianDateTimeString();
        public string Purchaser { get; set; }
        public string Status { get; set; }
        public string StatusTitle
        {
            get
            {
                if (Status == "5da43111-369a-4f7a-a024-6e470af1122f")
                    return "جدید";
                else if (Status == "ed9d3244-bc9f-4d67-8a24-5f580e951400")
                    return "تبدیل شده";
                else
                    return "نامشخص";
            }
        }
    }
}