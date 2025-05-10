using MArquise_Web.Model.DTOs.CRM;
using System.Threading.Tasks;

namespace Marquise_Web.Service.IService
{
    public interface IAccountService
    {
        Task<AccountDto> GetAccountWithManagerAsync(string crmId);
        Task<bool> UpdateAccountAsync(AccountDto account);
    }
}
