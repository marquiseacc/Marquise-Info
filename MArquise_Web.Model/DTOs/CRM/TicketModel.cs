using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MArquise_Web.Model.DTOs.CRM
{
    // Domain.DTOs/TicketDto.cs
    public class TicketDto
    {
        public string TicketId { get; set; }
        public string TicketNumber { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public string ITStaffId { get; set; }
        public string Status { get; set; }
    }

    // Domain.DTOs/StaffDto.cs
    public class StaffDto
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

}
