using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace Marquise_Web.Model.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string OtpCode { get; set; }
        public DateTime? OtpExpiration { get; set; }
        public Guid CRMId { get; set; }
        public string Gender { get; set; }
    }

    public enum Gender
    {
        Male,
        Female,
        Other
    }
}
