using System;
using System.ComponentModel.DataAnnotations;

namespace Marquise_Web.UI.areas.CRM.Models
{
    public class SendOtpVM
    {
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }

    public class VerifyOtpVM
    {
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(5, MinimumLength = 5)]
        public string Code { get; set; }
    }

    public class CRMCodeVM
    {
        [Required]
        public Guid CrmId { get; set; }
    }
}