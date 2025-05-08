

namespace Marquise_Web.UI.areas.CRM.Models
{
    public class MainDetailVM
    {
        public bool SupportStatus { get; set; }
        public int ActiveTicketNumber { get; set; }
        public int ActiveInvoiceNumber { get; set; }
        public double PaymentSum { get; set; }
    }

    public class SupportTimeVM
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}