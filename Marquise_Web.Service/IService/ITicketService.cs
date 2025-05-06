using MArquise_Web.Model.DTOs.CRM;
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
        Task<bool> CreateTicketAsync(NewTicketDto dto);
        Task<bool> AddAnswerAsync(NewAnswerDto dto);
        Task<bool> CloseTicketAsync(CloseTicketDto dto);
    }
}
