
using AutoMapper;
using Marquise_Web.Model.SiteModel;
using Marquise_Web.Service.IService;
using Marquise_Web.UI.Models;
using System;
using System.IO;
using System.Web;
using System.Web.Http;
using Utilities.Convert;

namespace Marquise_Web.UI.APIController
{

    public class MessageApiController : ApiController
    {
        private readonly SmtpSettings smtpSettings;
        private readonly IEmailService emailService;
        private readonly IMapper mapper;

        public MessageApiController(SmtpSettings smtpSettings, IEmailService emailService, IMapper mapper)
        {
            this.smtpSettings = smtpSettings;
            this.emailService = emailService;
            this.mapper = mapper;
        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/MessageApi/Contact")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public IHttpActionResult Contact([FromBody] ContactViewModel contactModel)
        {
            if (ModelState.IsValid)
            {
                var date = DateConvert.GetPersianDateTimeString(DateTime.Now);
                var email = "sdffbabaei@gmail.com";
                var subject = "پیام از سایت";
                var body = $@"
            <html>
                <body>
                    <p>با سلام،</p>
                    <p>امیدوارم روز خوبی داشته باشید.</p>
                    <p>شما یک پیام از {contactModel.Name} در ساعت {date} دریافت کرده اید.</p>
                    <p>متن پیام به شرح ذیل می باشد:</p>
                    <p>{contactModel.Message}</p>
                    <p>شماره تماس: {contactModel.PhoneNumber}</p>
                    <p>آدرس ایمیل: {contactModel.Email}</p>
                </body>
            </html>";

                using (var smtpClient = new System.Net.Mail.SmtpClient(smtpSettings.Host, smtpSettings.Port))
                {
                    smtpClient.Credentials = new System.Net.NetworkCredential(smtpSettings.Username, smtpSettings.Password);
                    smtpClient.EnableSsl = true;
                }

                MessageContactModel messageContactDto = new MessageContactModel()
                {
                    Name = contactModel.Name,
                    Phonenumber = contactModel.PhoneNumber,
                    Email = contactModel.Email,
                    MessageText = contactModel.Message,
                    Section ="ContactUs"
                };
                var check = emailService;
                emailService.ContactEmailService(smtpSettings, messageContactDto, email, body, subject);
                
                return Json(new { success = true });
            }

            else return Json(new { success = false });
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/MessageApi/Opportunity")]
        public IHttpActionResult Opportunity([FromBody] MessageViewModel messageViewModel)
        {
            if (ModelState.IsValid)
            {
                // تبدیل فایل از Base64 به بایت آرایه
                var fileBytes = Convert.FromBase64String(messageViewModel.File);
                var date = DateConvert.GetPersianDateTimeString(DateTime.Now);
                var email = "sdffbabaei@gmail.com";
                var subject = "پیام از سایت";
                var body = $@"
            <html>
                <body>
                    <p>با سلام،</p>
                    <p>امیدوارم روز خوبی داشته باشید.</p>
                    <p>شما یک پیام از {messageViewModel.Name} در ساعت {date} دریافت کرده اید.</p>
                    <p>متن پیام به شرح ذیل می باشد:</p>
                    <p>{messageViewModel.Message}</p>
                    <p>شماره تماس: {messageViewModel.PhoneNumber}</p>
                    <p>آدرس ایمیل: {messageViewModel.Email}</p>
                </body>
            </html>";

                var timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                var fileExtension = Path.GetExtension(messageViewModel.FileName); 
                var fileName = $"{timeStamp}_{messageViewModel.Name}{fileExtension}";
                var filePath = HttpContext.Current.Server.MapPath($"~/Content/Resume/{fileName}");
                File.WriteAllBytes(filePath, fileBytes);

                var messageModel = mapper.Map<MessageModel>(messageViewModel);
                messageModel.FilePath = filePath;
                // ارسال فایل به لایه سرویس
                emailService.OpportunityEmailService(
                    smtpSettings,
                    messageModel,
                    email,
                    body,
                    subject,
                    fileBytes, // ارسال فایل به صورت بایت آرایه
                    messageViewModel.FileName
                );
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }




    }
}
