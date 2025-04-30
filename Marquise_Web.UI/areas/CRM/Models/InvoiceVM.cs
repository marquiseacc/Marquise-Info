using System;
using Utilities.Convert;

namespace Marquise_Web.UI.areas.CRM.Models
{
    public class InvoiceVM
    {
        public string InvoiceId { get; set; }
        public int InvoiceNumber { get; set; }
        public string Title { get; set; }
        public double TotalPrice { get; set; }
        public double FinalAmount { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateDatePersian => CreateDate.ToPersianDateString();
        public string Status { get; set; }
        public string StatusTitle
        {
            get
            {
                if (Status == "defe0fbb-f568-49d8-bd65-f74781635da2")
                    return "تایید";
                else if (Status == "2190e6ec-d127-48b1-953e-70cc8812e986")
                    return "باز";
                else if (Status == "9e771182-851a-4417-accf-e5e1ea5167b9")
                    return "پرداخت شده";
                //else if (Status == "")
                //    return "ابطال";
                else
                    return "نامشخص";
            }
        }
    }

    public class InvoiceDetailVm
    {
        public string InvoiceId { get; set; }
        public int InvoiceNumber { get; set; }
        public string Title { get; set; }
        public double TotalPrice { get; set; }
        public double FinalAmount { get; set; }
        public double TotalDiscount { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateDatePersian => CreateDate.ToPersianDateTimeString();
        public double TotalTax { get; set; }
        public string Status { get; set; }
        public string StatusTitle
        {
            get
            {
                if (Status == "defe0fbb-f568-49d8-bd65-f74781635da2")
                    return "تایید";
                else if (Status == "2190e6ec-d127-48b1-953e-70cc8812e986")
                    return "باز";
                else if (Status == "9e771182-851a-4417-accf-e5e1ea5167b9")
                    return "پرداخت شده";
                //else if (Status == "")
                //    return "ابطال";
                else
                    return "نامشخص";
            }
        }
    }
}