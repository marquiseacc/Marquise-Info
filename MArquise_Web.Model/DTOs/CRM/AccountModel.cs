using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MArquise_Web.Model.DTOs.CRM
{
    public class AccountDto
    {
        public Guid AccountId { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string IndustryCode { get; set; }
        public string ShippingAddress { get; set; }
        public string Mobile { get; set; }
        public double shomaremoshtari__C { get; set; }
        public string mahale__C { get; set; }
        public string cituu__C { get; set; }
        public string management__C { get; set; }
        public string ManagementName { get; set; } // پر می‌شه از contact
    }

    public class ContactDto
    {
        public Guid ContactId { get; set; }
        public string FullName { get; set; }
    }

    public class AccountApiResponse
    {
        public bool Succeeded { get; set; }
        public List<string> ResultMessageList { get; set; }
        public object ResultId { get; set; }
        public object ErrorCode { get; set; }
        public AccountResultData ResultData { get; set; }
    }

    public class AccountResultData
    {
        public List<AccountDto> result { get; set; }
        public object TotalRows { get; set; }
    }

    public class ContactResultData
    {
        public List<ContactDto> result { get; set; }
        public object TotalRows { get; set; }
    }

    public class ContactApiResponse
    {
        public bool Succeeded { get; set; }
        public List<string> ResultMessageList { get; set; }
        public object ResultId { get; set; }
        public object ErrorCode { get; set; }
        public ContactResultData ResultData { get; set; }
    }
}
