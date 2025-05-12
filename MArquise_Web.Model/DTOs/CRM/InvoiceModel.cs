using System;
using System.Collections.Generic;

namespace Marquise_Web.Model.DTOs.CRM
{
    public class InvoiceDto
    {
        public string InvoiceId { get; set; }
        public int InvoiceNumber { get; set; }
        public string Title { get; set; }
        public double TotalPrice { get; set; }
        public double FinalAmount { get; set; }
        public double Rest { get; set; }
        public double Paid { get; set; }
        public DateTime CreateDate { get; set; }
        public string Status { get; set; }
        public string AccountId { get; set; }
    }

    public class InvoiceDetailDto : InvoiceDto
    {
        public double TotalDiscount { get; set; }
        public double TotalTax { get; set; }
        public List<PaymentDto> Payments { get; set; }
    }

    public class PaymentDto
    {
        public string Number { get; set; }
        public double Amount { get; set; }
        public string Type { get; set; }
        public DateTime PaymentDate { get; set; }
        public string InvoiceId { get; set; }
        public string AccountId { get; set; }
    }

}
