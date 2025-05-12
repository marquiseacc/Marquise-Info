using System;

namespace Marquise_Web.Model.DTOs.CRM
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

    // TicketDetailDto.cs
    public class TicketDetailDto
    {
        public string TicketId { get; set; }
        public string TicketNumber { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public string ITStaffId { get; set; }
        public string Status { get; set; }
    }

    // AnswerDto.cs
    public class AnswerDto
    {
        public string Id { get; set; }
        public string TicketId { get; set; }
        public string Body { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class NewTicketDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string CrmId { get; set; }
    }

    public class NewAnswerDto
    {
        public string TicketId { get; set; }
        public string Message { get; set; }
    }

    public class CloseTicketDto
    {
        public string TicketId { get; set; }
    }

}
