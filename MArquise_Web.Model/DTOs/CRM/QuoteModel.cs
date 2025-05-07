using System;

namespace MArquise_Web.Model.DTOs.CRM
{
    public class QuoteDto
    {
        public string QuoteId { get; set; }
        public int QuoteCode { get; set; }
        public string Name { get; set; }
        public double TotalPrice { get; set; }
        public double FinalAmount { get; set; }
        public DateTime CreateDate { get; set; }
        public string Status { get; set; }
        public string AccountId { get; set; }
    }

    public class QuoteDetailDto : QuoteDto
    {
        public double TotalDiscount { get; set; }
        public string Purchaser { get; set; }
    }

}
