using System;
using System.Collections.Generic;
using Utilities.Convert;

namespace Marquise_Web.UI.areas.CRM.Models
{
    public class InvoiceVM
    {
        public string InvoiceId { get; set; }
        public int InvoiceNumber { get; set; }
        public string Title { get; set; }
        public double TotalPrice { get; set; }
        public double FinalAmount { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateDatePersian => CreateDate.ToPersianDateString();
        public string Status { get; set; }
        public string StatusTitle
        {
            get
            {
                if (Status == "defe0fbb-f568-49d8-bd65-f74781635da2")
                    return "تایید";
                else if (Status == "2190e6ec-d127-48b1-953e-70cc8812e986")
                    return "باز";
                else if (Status == "9e771182-851a-4417-accf-e5e1ea5167b9")
                    return "پرداخت شده";
                else if (Status == "9d0bce73-e071-4c6f-a64e-1fed6bd7df74")
                    return "ابطال";
                else
                    return "نامشخص";
            }
        }
    }

    public class InvoiceDetailVm
    {
        public string InvoiceId { get; set; }
        public int InvoiceNumber { get; set; }
        public string Title { get; set; }
        public double TotalPrice { get; set; }
        public double FinalAmount { get; set; }
        public double TotalDiscount { get; set; }
        public double Rest { get; set; }
        public double Paid { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateDatePersian => CreateDate.ToPersianDateTimeString();
        public double TotalTax { get; set; }
        public List<PaymentVM> Payments { get; set; }
        public string Status { get; set; }
        public string StatusTitle
        {
            get
            {
                if (Status == "defe0fbb-f568-49d8-bd65-f74781635da2")
                    return "تایید";
                else if (Status == "2190e6ec-d127-48b1-953e-70cc8812e986")
                    return "باز";
                else if (Status == "9e771182-851a-4417-accf-e5e1ea5167b9")
                    return "پرداخت شده";
                else if (Status == "9d0bce73-e071-4c6f-a64e-1fed6bd7df74")
                    return "ابطال";
                else
                    return "نامشخص";
            }
        }
    }

    public class PaymentVM
    {
        public string Number { get; set; }
        public double Amount { get; set; }
        public string Type { get; set; }
        public string TypeName
        {
            get
            {
                switch (Type)
                {
                    case "949bdca6-f1cb-4077-a409-6b2380844a8a":
                        return "کارت به کارت";
                    case "f62f0e57-8236-4714-9492-6d51a3226bcc":
                        return "واریز به حساب بانکی رسمی(اقتصاد نوین)";
                    case "31642864-e966-4e2c-afa3-aedecd452768":
                        return "غير آنلاين";
                    case "e8504153-bbbc-4071-8332-d9da48748fba":
                        return "واریز به حساب بانکی رسمی(پارسیان)";
                    case "301b923a-f31a-4fcd-a28c-e3a41a72c50a":
                        return "آنلاين";
                    case "560f1634-7405-402c-98bf-ef1077b94546":
                        return "واریز به حساب بانکی غیر رسمی(ملت)";
                    case "cc7cfc15-9c64-487b-97ac-fcd2b0de11e0":
                        return "واریز به حساب بانکی غیر رسمی(سایر)";
                    default:
                        return "نامشخص";
                }
            }
        }

        public DateTime PaymentDate { get; set; }
        public string PaymentDatePersian => PaymentDate.ToPersianDateString();
        public string InvoiceId { get; set; }
        public string AccountId { get; set; }
    }

}