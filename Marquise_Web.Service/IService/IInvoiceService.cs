using MArquise_Web.Model.DTOs.CRM;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marquise_Web.Service.IService
{
    public interface IInvoiceService
    {
        Task<List<InvoiceDto>> GetInvoicesByAccountIdAsync(string accountId);
        Task<InvoiceDetailDto> GetInvoiceDetailAsync(string invoiceId);
    }
}
