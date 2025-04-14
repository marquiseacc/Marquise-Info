

using Marquise_Web.Model.DTOs.SiteModel;

namespace Marquise_Web.Service.IService
{
    public interface IMessageService
    {
        void ContactEmailService(SmtpSettings smtpSettings, MessageContactDTO messageContactDto, string emailAddress, string emailBody, string emailSubjet);
        void OpportunityEmailService(SmtpSettings smtpSettings, MessageDTO messageDto, string emailAddress, string emailBody, string emailSubjet, byte[] fileBytes, string filename);
    }
}
