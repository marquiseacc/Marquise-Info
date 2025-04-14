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
        [StringLength(6, MinimumLength = 6)]
        public string Code { get; set; }
    }
}