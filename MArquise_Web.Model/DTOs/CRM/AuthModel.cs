using Marquise_Web.Model.Entities;
using System;

namespace Marquise_Web.Model.DTOs.CRM
{
    public class OtpRequestLog
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RequestTime { get; set; }
        public string IPAddress { get; set; } // اختیاری ولی مفید

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }

}
