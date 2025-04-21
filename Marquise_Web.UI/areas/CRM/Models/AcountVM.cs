using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Marquise_Web.UI.areas.CRM.Models
{
    public class ApiResponse
    {
        public bool Succeeded { get; set; }
        public List<string> ResultMessageList { get; set; }
        public object ResultId { get; set; }
        public string ErrorCode { get; set; }
        public ResultData ResultData { get; set; }
    }


    public class ResultData
    {
        public List<ContactModel> result { get; set; }
        public int? TotalRows { get; set; }
    }


    public class ContactModel
    {
        public string ContactId { get; set; }
        public string FullName { get; set; }
        public string FullName_FirstName { get; set; }
        public string FullName_LastName { get; set; }
        public string FullName_NamePrefix { get; set; }
        public string AvatarPath { get; set; }
        public string ITStaffID { get; set; }
        public string AccountId { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string UserId { get; set; }
        public string Address { get; set; }
        public string Address_ProvinceId { get; set; }
        public string Address_CityId { get; set; }
        public string Address_Street { get; set; }
        public string Address_PostalCode { get; set; }
        public string Birthday { get; set; }
        public string MarriageDate { get; set; }
        public string TemplateAttributeId { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public bool IsRemoved { get; set; }
        public string IsApprovalLock { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastActivityDate { get; set; }
        public string RecordColor { get; set; }
        public string NationalCode { get; set; }
        public string jensiat__C { get; set; }
        public string tarikhemoshtarishodan__C { get; set; }
        public string mizanedaramad__C { get; set; }
        public string tahsilat__C { get; set; }
        public string ravannegari__C { get; set; }
        public string lifestyle__C { get; set; }
        public string arzeshha__C { get; set; }
        public string personality__C { get; set; }
        public string fatrarbasherkat__C { get; set; }
        public string nahveashnaeeee__C { get; set; }
        public string telephooone__C { get; set; }
    }

}