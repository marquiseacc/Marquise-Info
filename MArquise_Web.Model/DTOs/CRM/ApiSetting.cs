

namespace Marquise_Web.Model.DTOs.CRM
{
    public class CRMApiSetting
    {
        public string ApiBaseUrl { get; set; }
        public string ApiToken { get; set; }
    }

    public class SMSApiSetting
    {
        public string ApiBaseUrl { get; set; }
        public string ApiKey { get; set; }
        public string ApiTemplateId { get; set; }
    }
}
