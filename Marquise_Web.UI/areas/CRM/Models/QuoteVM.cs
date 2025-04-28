using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        public string Purchaser { get; set; }
    }
}