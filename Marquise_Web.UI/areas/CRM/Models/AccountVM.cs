using System;
using System.Collections.Generic;
using System.Linq;

namespace Marquise_Web.UI.areas.CRM.Models
{
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
        public List<AccountVM> result { get; set; }
        public object TotalRows { get; set; }
    }

    public class AccountVM
    {
        public Guid AccountId { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string IndustryCode { get; set; }
        public string IndustryTitle => Industry.GetTitle(IndustryCode);
        public string ShippingAddress { get; set; }
        public string Mobile { get; set; }
        public double shomaremoshtari__C { get; set; }
        public string mahale__C { get; set; }
        public string cituu__C { get; set; }
        public string management__C { get; set; }
        public string ManagementName { get; set; }
    }

    public class Industry
    {
        public string Id { get; set; }
        public string Title { get; set; }

        public static List<Industry> GetAll() => new List<Industry>
    {
        new Industry { Id = "759302ac-5a4e-4a44-a5bb-92a1cea1b488", Title = "خرده فروش" },
        new Industry { Id = "e685c0e4-2606-4307-b763-456d0d17426a", Title = "بنکدار" },
        new Industry { Id = "a6479a35-6e51-49c3-9e4d-1ee207d11d80", Title = "کیفی" },
        new Industry { Id = "bc74e815-8519-4b8c-bd5e-a510b0cbb267", Title = "سکه فروش" },
        new Industry { Id = "7af8fdf0-570f-4a8b-8f68-21b648c8360f", Title = "آبشده فروش" },
        new Industry { Id = "6eb7c362-2050-40cc-ac12-0c492457f51f", Title = "سنگ فروش" },
        new Industry { Id = "067977ac-c5f7-4220-aecf-9c7ccfd0ef80", Title = "خدمات زرگری" },
        new Industry { Id = "04906e01-2541-4d23-a813-9235b30bd9ee", Title = "نقره فروش" },
        new Industry { Id = "1eb05f3d-e76e-4fb0-98a0-ae8467cc1769", Title = "صرافی" },
        new Industry { Id = "9e1814c2-ade9-40ca-8b9f-b50e17f1b775", Title = "مشاوره" },
        new Industry { Id = "c9acc41f-db23-4f64-8c83-efdf47b05c1f", Title = "آموزش" },
        new Industry { Id = "090903a7-9813-423d-92dc-06bd364bd380", Title = "سایر" },
    };

        public static string GetTitle(string id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id)?.Title ?? "نامشخص";
        }
    }


    public class ContactApiResponse
    {
        public bool Succeeded { get; set; }
        public List<string> ResultMessageList { get; set; }
        public object ResultId { get; set; }
        public object ErrorCode { get; set; }
        public ContactResultData ResultData { get; set; }
    }

    public class ContactResultData
    {
        public List<ContactVM> result { get; set; }
        public object TotalRows { get; set; }
    }

    public class ContactVM
    {
        public Guid ContactId { get; set; }
        public string FullName { get; set; }
    }
}