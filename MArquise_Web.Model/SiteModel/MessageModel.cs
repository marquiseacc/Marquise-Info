using System;

namespace Marquise_Web.Model.SiteModel
{
    public class MessageModel
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
        public string Message { get; set; }
    }
}
