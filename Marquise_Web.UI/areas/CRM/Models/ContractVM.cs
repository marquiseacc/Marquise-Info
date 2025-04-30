using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilities.Convert;

namespace Marquise_Web.UI.areas.CRM.Models
{
    public class ContractVM
    {
        public string Id { get; set; }
        public string sodoureghoflName__C { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateDatePersian => CreateDate.ToPersianDateString();
        public string status__C { get; set; }
        public string StatusTitle
        {
            get
            {
                if (status__C == "73bbad0b-6e0f-402b-a581-82a87620dbd7")
                    return "فعال";
                else if (status__C == "66e1823c-c99c-451a-845a-2dec96750754")
                    return "غیرفعال";
                else
                    return "نامشخص";
            }
        }
    }

    public class ContractDetailVm
    {
        public string Id { get; set; }
        public string sodoureghoflName__C { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateDatePersian => CreateDate.ToPersianDateString();
        public DateTime dateend__C { get; set; }
        public string EndDatePersian => dateend__C.ToPersianDateString();
        public DateTime datestart__C { get; set; }
        public string StartDatePersian => datestart__C.ToPersianDateString();
        public bool tamdidgharardad__C { get; set; }
        public string tamdidStatus {  get; set; }
        public string status__C { get; set; }
        public string StatusTitle
        {
            get
            {
                if (status__C == "73bbad0b-6e0f-402b-a581-82a87620dbd7")
                    return "فعال";
                else if (status__C == "66e1823c-c99c-451a-845a-2dec96750754")
                    return "غیرفعال";
                else
                    return "نامشخص";
            }
        }
    }
}