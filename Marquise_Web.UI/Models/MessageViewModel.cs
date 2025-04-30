using System;

namespace Marquise_Web.UI.Models
{
    public class MessageViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string File { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public DateTime Birthday { get; set; }
        public string Address { get; set; }
        public string Section { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Message { get; set; }
    }
}