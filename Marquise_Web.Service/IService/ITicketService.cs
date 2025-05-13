using Marquise_Web.Model.DTOs.CRM;
using Marquise_Web.Model.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marquise_Web.Service.IService
{
    public interface ITicketService
    {
        Task<List<TicketDto>> GetTicketsByApplicantIdAsync(string crmId);
        Task<TicketDetailDto> GetTicketByIdAsync(string ticketId);
        Task<List<AnswerDto>> GetAnswersByTicketIdAsync(string ticketId);
        Task<List<StaffDto>> GetAllStaffAsync();
        Task<OperationResult<object>> CreateTicketAsync(NewTicketDto dto);
        Task<OperationResult<object>> AddAnswerAsync(NewAnswerDto dto);
        Task<OperationResult<object>> CloseTicketAsync(CloseTicketDto dto);
    }
}
