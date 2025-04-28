using System;
using System.Collections.Generic;

namespace Marquise_Web.UI.areas.CRM.Models
{
    public class ApiResponse
    {
    public bool Succeeded { get; set; }
        public List<string> ResultMessageList { get; set; }
        public object ResultId { get; set; }
        public object ErrorCode { get; set; }
        public ResultData ResultData { get; set; }
    }

    public class ResultData
    {
        public List<ResultItem> result { get; set; }
        public object TotalRows { get; set; }
    }

    public class ResultItem
    {
        public Guid AccountId { get; set; }
        public string Name { get; set; }
        public Guid ITStaffID { get; set; }
        public string Type { get; set; }
        public string WebSiteUrl { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public Guid IndustryCode { get; set; }
        public double PersonnelCount { get; set; }
        public string Description { get; set; }
        public double EconomicCode { get; set; }
        public double NationalCode { get; set; }
        public Guid? ParentID { get; set; }
        public string LogoPath { get; set; }
        public string ShippingAddress { get; set; }
        public Guid? ShippingAddress_ProvinceId { get; set; }
        public Guid? ShippingAddress_CityId { get; set; }
        public string ShippingAddress_Street { get; set; }
        public string ShippingAddress_PostalCode { get; set; }
        public string BillAddress { get; set; }
        public Guid? BillAddress_ProvinceId { get; set; }
        public Guid? BillAddress_CityId { get; set; }
        public string BillAddress_PostalCode { get; set; }
        public string BillAddress_Street { get; set; }
        public Guid TemplateAttributeId { get; set; }
        public Guid CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public bool IsRemoved { get; set; }
        public string IsApprovalLock { get; set; }
        public Guid ApprovalStatus { get; set; }
        public object WorkingHours { get; set; }
        public DateTime? LastActivityDate { get; set; }
        public string RecordColor { get; set; }
        public double Rest { get; set; }
        public double Paid { get; set; }
        public double SumInvoices { get; set; }
        public Guid IdentityType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public Guid? BillAddress_ZoneId { get; set; }
        public double shomaremoshtari__C { get; set; }
        public object gharardaad__C { get; set; }
        public Guid MizaneDaramad__C { get; set; }
        public Guid MIzanedaramdzaee__C { get; set; }
        public Guid Raftarkharidmoshtari__C { get; set; }
        public double Mantaghe__C { get; set; }
        public string mahale__C { get; set; }
        public string cituu__C { get; set; }
        public double rezayatemoshtari__C { get; set; }
        public double narezayateemoshtari__C { get; set; }
        public object needss__C { get; set; }
        public Guid dalilerezayat__C { get; set; }
        public Guid dalilenarezayatee__C { get; set; }
        public string nahveashnaee__C { get; set; }
        public string sodoureghoflName__C { get; set; }
        public string seryalmarkiz__C { get; set; }
        public object sathedaramadmoshtari__C { get; set; }
        public string telephoone__C { get; set; }
        public Guid management__C { get; set; }
        public object moearef__C { get; set; }
    }

    public class AccountVM
    {
        public string ManagementName { get; set; }
        public string GalleryName { get; set; }
        public string Industry { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
    }
}