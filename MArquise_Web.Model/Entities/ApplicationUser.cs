using Marquise_Web.Model.DTOs.CRM;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;

namespace Marquise_Web.Model.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string OtpCode { get; set; }
        public DateTime? OtpExpiration { get; set; }

        public virtual ICollection<OtpRequestLog> OtpRequestLogs { get; set; }
        public virtual ICollection<OtpVerifyLog> OtpVerifyLogs { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
    }

}
