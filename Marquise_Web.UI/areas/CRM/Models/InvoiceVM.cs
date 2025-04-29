using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        public double TotalTax { get; set; }
    }
}