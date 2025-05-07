using MArquise_Web.Model.DTOs.CRM;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marquise_Web.Service.IService
{
    public interface IQuoteService
    {
        Task<List<QuoteDto>> GetQuotesByAccountIdAsync(string crmId);
        Task<QuoteDetailDto> GetQuoteDetailAsync(string quoteId);
    }
}
