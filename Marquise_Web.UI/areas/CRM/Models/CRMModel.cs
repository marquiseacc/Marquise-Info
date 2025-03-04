using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Marquise_Web.UI.areas.CRM.Models
{
    public class ResultData
    {
        public List<Result> result { get; set; }
        public int? TotalRows { get; set; }
    }

    public class Result
    {
        public Guid AccountId { get; set; }
        public string Name { get; set; }
        public Guid ITStaffID { get; set; }
        public Guid Type { get; set; }
        public string WebSiteUrl { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string IndustryCode { get; set; }
        public double? PersonnelCount { get; set; }
        public string Description { get; set; }
        public double? EconomicCode { get; set; }
        public double? NationalCode { get; set; }
        public string ParentID { get; set; }
        public string LogoPath { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingAddress_ProvinceId { get; set; }
        public string ShippingAddress_CityId { get; set; }
        public string ShippingAddress_Street { get; set; }
        public string ShippingAddress_PostalCode { get; set; }
        public string BillAddress { get; set; }
        public string BillAddress_ProvinceId { get; set; }
        public string BillAddress_CityId { get; set; }
        public string BillAddress_Street { get; set; }
        public string BillAddress_PostalCode { get; set; }
        public Guid TemplateAttributeId { get; set; }
        public Guid CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }


        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("{");
            sb.AppendLine($"  \"AccountId\": \"{AccountId}\",");
            sb.AppendLine($"  \"Name\": \"{Name}\",");
            sb.AppendLine($"  \"ITStaffID\": \"{ITStaffID}\",");
            sb.AppendLine($"  \"Type\": \"{Type}\",");
            sb.AppendLine($"  \"WebSiteUrl\": \"{WebSiteUrl}\",");
            sb.AppendLine($"  \"Telephone\": \"{Telephone}\",");
            sb.AppendLine($"  \"Fax\": \"{Fax}\",");
            sb.AppendLine($"  \"IndustryCode\": \"{IndustryCode}\",");
            sb.AppendLine($"  \"PersonnelCount\": {PersonnelCount?.ToString() ?? "null"},");

            sb.AppendLine($"  \"Description\": \"{Description}\",");
            sb.AppendLine($"  \"EconomicCode\": {EconomicCode?.ToString() ?? "null"},");

            sb.AppendLine($"  \"NationalCode\": {NationalCode?.ToString() ?? "null"},");
            sb.AppendLine($"  \"ParentID\": \"{ParentID}\",");
            sb.AppendLine($"  \"LogoPath\": \"{LogoPath}\",");
            sb.AppendLine($"  \"ShippingAddress\": \"{ShippingAddress}\",");
            sb.AppendLine($"  \"ShippingAddress_ProvinceId\": \"{ShippingAddress_ProvinceId}\",");
            sb.AppendLine($"  \"ShippingAddress_CityId\": \"{ShippingAddress_CityId}\",");
            sb.AppendLine($"  \"ShippingAddress_Street\": \"{ShippingAddress_Street}\",");
            sb.AppendLine($"  \"ShippingAddress_PostalCode\": \"{ShippingAddress_PostalCode}\",");
            sb.AppendLine($"  \"BillAddress\": \"{BillAddress}\",");
            sb.AppendLine($"  \"BillAddress_ProvinceId\": \"{BillAddress_ProvinceId}\",");
            sb.AppendLine($"  \"BillAddress_CityId\": \"{BillAddress_CityId}\",");
            sb.AppendLine($"  \"BillAddress_Street\": \"{BillAddress_Street}\",");
            sb.AppendLine($"  \"BillAddress_PostalCode\": \"{BillAddress_PostalCode}\",");
            sb.AppendLine($"  \"TemplateAttributeId\": \"{TemplateAttributeId}\",");
            sb.AppendLine($"  \"CreateBy\": \"{CreateBy}\",");
            sb.AppendLine($"  \"CreateDate\": \"{CreateDate:yyyy-MM-ddTHH:mm:ss}\",");
            sb.AppendLine($"  \"ModifyBy\": \"{ModifyBy}\",");
            sb.AppendLine($"  \"ModifyDate\": \"{ModifyDate:yyyy-MM-ddTHH:mm:ss}\"");
            sb.AppendLine("}");

            return sb.ToString();
        }
    }

    

    public class ApiResponse
    {
        public bool Succeeded { get; set; }
        public List<string> ResultMessageList { get; set; }
        public string ResultId { get; set; }
        public string ErrorCode { get; set; }
        public ResultData ResultData { get; set; }
    }
}