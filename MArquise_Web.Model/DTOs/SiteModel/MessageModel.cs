using System;

namespace Marquise_Web.Model.DTOs.SiteModel
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string FilePath { get; set; }
        public DateTime Birthday { get; set; }
        public string Address { get; set; }
        public string Section { get; set; }
        public DateTime RegisterDate { get; set; }
        public string MessageText { get; set; }
        public string EmailBody { get; set; }
        public string EmailAddress { get; set; }
        public string EmailSubject { get; set; }
    }

    public class MessageContactDTO
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string MessageText { get; set; }
        public string Section { get; set; }
        public string EmailBody { get; set; }
        public string EmailAddress { get; set; }
        public string EmailSubject { get; set; }
    }


    public class SmtpSettings
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
    }
}
