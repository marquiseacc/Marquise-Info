using System;

namespace Marquise_Web.Model.DTOs.CRM
{
    public class AuthUserDto
    {
        public string Id { get; set; }
        public string PhoneNumber { get; set; }
        public Guid CRMId { get; set; }
    }
}
