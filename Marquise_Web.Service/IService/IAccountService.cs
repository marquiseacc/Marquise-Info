using Marquise_Web.Model.DTOs.CRM;
using Marquise_Web.Model.Utilities;
using System.Threading.Tasks;

namespace Marquise_Web.Service.IService
{
    public interface IAccountService
    {
        Task<CrmAccountDto> GetAccountWithManagerAsync(string crmId);
        Task<OperationResult<object>> UpdateAccountAsync(CrmAccountDto account);
    }
}
