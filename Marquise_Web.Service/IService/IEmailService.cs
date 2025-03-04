using Marquise_Web.Model.SiteModel;

namespace Marquise_Web.Service.IService
{
    public interface IEmailService
    {
        void ContactEmailService(SmtpSettings smtpSettings, MessageContactModel messageContactDto, string emailAddress, string emailBody, string emailSubjet);
        void OpportunityEmailService(SmtpSettings smtpSettings, MessageModel messageDto, string emailAddress, string emailBody, string emailSubjet, byte[] fileBytes, string filename);
    }
}
