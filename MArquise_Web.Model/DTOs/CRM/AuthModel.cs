using Marquise_Web.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marquise_Web.Model.DTOs.CRM
{
    public class OtpRequestLog
    {
        [MaxLength(50)]
        public string Id { get; set; }
        [MaxLength(50)]
        public string PhoneNumber { get; set; }
        public DateTime RequestTime { get; set; }
        [MaxLength (100)]
        public string IPAddress { get; set; } // اختیاری ولی مفید

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }

    public class OtpVerifyLog
    {
        [MaxLength(50)]
        public string Id { get; set; }
        [MaxLength(50)]
        public string PhoneNumber { get; set; }
        public DateTime TryTime { get; set; }
        public bool IsSuccess { get; set; }
        [MaxLength(100)]
        public string IPAddress { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }

    public class Account
    {
        [MaxLength(50)]
        public string Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string CrmAccountId { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string CrmParentId { get; set; }
        [ForeignKey("Parent")]
        public string ParentId { get; set; }
        public virtual Account Parent { get; set; }
    }

}
