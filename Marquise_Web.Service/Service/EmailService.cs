using Marquise_Web.Data;
using Marquise_Web.Data.IRepository;
using Marquise_Web.Service.IService;
using System.Net.Mail;
using System.IO;
using AutoMapper;
using Marquise_Web.Model.SiteModel;
using System.Net;

namespace Marquise_Web.Service.Service
{
    public class EmailService : IEmailService
    {
        private readonly IMessageRepository messageRepository;
        private readonly IMapper mapper;

        public EmailService(IMessageRepository messageRepository, IMapper mapper)
        {
            this.messageRepository = messageRepository;
            this.mapper = mapper;
        }

        public void ContactEmailService(SmtpSettings smtpSettings, MessageContactModel messageContactDto, string emailAddress, string emailBody, string emailSubjet)
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
                Message message = mapper.Map<Message>(messageContactDto);
                messageRepository.Add(message);
            }
        }

        public void OpportunityEmailService(SmtpSettings smtpSettings, MessageModel messageDto, string emailAddress, string emailBody, string emailSubjet, byte[] fileBytes, string filename)
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
                Message message = mapper.Map<Message>(messageDto);
                messageRepository.Add(message);
            }
        }
    }
}
