using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marquise_Web.Model.CRM
{
    public class CostumerCRMModel
    {
        public int UserId { get; set; }
        public Guid? AccountId { get; set; }
        public Guid DepartmentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UserType { get; set; }
        public string Tel { get; set; }
        public string TelExt { get; set; }
        public string Address { get; set; }
        public string MobilePhone { get; set; }
        public bool IsActiveDirectory { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid ModifierUser { get; set; }
        public string IPAddress { get; set; }
        public int EnableNotification { get; set; }
        public string PersonalCode { get; set; }
        public bool EnableVoip { get; set; }
        public string DisplayFeedBack { get; set; }
        public Guid DomainId { get; set; }
        public Guid? PositionId { get; set; }
        public int Status { get; set; }
        public bool IsRemoved { get; set; }
        public bool SendInvitationEmail { get; set; }
        public bool IsApproved { get; set; }
        public string Email { get; set; }
        public bool IsLockedOut { get; set; }
        public string UserName { get; set; }
    
    public override string ToString()
        {
            return $@"
    {{
        ""UserId"": ""{UserId}"",
        ""AccountId"": {(AccountId.HasValue ? $"\"{AccountId}\"" : "null")},
        ""DepartmentID"": ""{DepartmentID}"",
        ""FirstName"": ""{FirstName}"",
        ""LastName"": ""{LastName}"",
        ""UserType"": {UserType},
        ""Tel"": ""{Tel}"",
        ""TelExt"": ""{TelExt}"",
        ""Address"": ""{Address}"",
        ""MobilePhone"": ""{MobilePhone}"",
        ""IsActiveDirectory"": {IsActiveDirectory.ToString().ToLower()},
        ""CreateDate"": ""{CreateDate:yyyy-MM-ddTHH:mm:ss.fff}"",
        ""ModifierUser"": ""{ModifierUser}"",
        ""IPAddress"": ""{IPAddress}"",
        ""EnableNotification"": {EnableNotification},
        ""PersonalCode"": ""{PersonalCode}"",
        ""EnableVoip"": {EnableVoip.ToString().ToLower()},
        ""DisplayFeedBack"": {(string.IsNullOrEmpty(DisplayFeedBack) ? "null" : $"\"{DisplayFeedBack}\"")},
        ""DomainId"": ""{DomainId}"",
        ""PositionId"": {(PositionId.HasValue ? $"\"{PositionId}\"" : "null")},
        ""Status"": {Status},
        ""IsRemoved"": {IsRemoved.ToString().ToLower()},
        ""SendInvitationEmail"": {SendInvitationEmail.ToString().ToLower()},
        ""IsApproved"": {IsApproved.ToString().ToLower()},
        ""Email"": ""{Email}"",
        ""IsLockedOut"": {IsLockedOut.ToString().ToLower()},
        ""UserName"": ""{UserName}""
    }}";
        }

    }
}
