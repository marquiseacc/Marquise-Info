using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Marquise_Web.UI.areas.CRM.Models
{
    public class ContractVM
    {
        public string Id { get; set; }
        public string sodoureghoflName__C { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class ContractDetailVm
    {
        public string Id { get; set; }
        public string sodoureghoflName__C { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime dateend__C { get; set; }
        public DateTime datestart__C { get; set; }
        public bool tamdidgharardad__C { get; set; }
        public string tamdidStatus {  get; set; }

    }
}