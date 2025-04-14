using Marquise_Web.Data;
using Marquise_Web.Data.IRepository;
using Marquise_Web.Service.IService;
using System.Net.Mail;
using System.IO;
using System.Net;
using Utilities.Map;
using Marquise_Web.Model.DTOs.SiteModel;

namespace Marquise_Web.Service.Service
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWorkRepository unitOfWork;

        public MessageService(IUnitOfWorkRepository unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void ContactEmailService(SmtpSettings smtpSettings, MessageContactDTO messageContactDto, string emailAddress, string emailBody, string emailSubjet)
        {
            using (var smtpClient = new SmtpClient(smtpSettings.Host, smtpSettings.Port))
            {

                smtpClient.Credentials = new NetworkCredential(smtpSettings.Username, smtpSettings.Password);
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpSettings.From),
                    Subject = emailSubjet,
                    Body = emailBody,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(emailAddress);
                smtpClient.Send(mailMessage);
                Message message = DataMapper.Mapper.Map<Message>(messageContactDto);
                unitOfWork.MessageRepository.Add(message);
            }
        }

        public void OpportunityEmailService(SmtpSettings smtpSettings, MessageDTO messageDto, string emailAddress, string emailBody, string emailSubjet, byte[] fileBytes, string filename)
        {
            using (var smtpClient = new SmtpClient(smtpSettings.Host, smtpSettings.Port))
            {

                smtpClient.Credentials = new NetworkCredential(smtpSettings.Username, smtpSettings.Password);
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpSettings.From),
                    Subject = emailSubjet,
                    Body = emailBody,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(emailAddress);

                if (fileBytes != null && fileBytes.Length > 0)
                {
                    var attachment = new Attachment(new MemoryStream(fileBytes), filename);
                    mailMessage.Attachments.Add(attachment);
                }



                smtpClient.Send(mailMessage);
                Message message = DataMapper.Mapper.Map<Message>(messageDto);
                unitOfWork.MessageRepository.Add(message);
            }
        }
    }
}
