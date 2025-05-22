using Marquise_Web.Model.DTOs.CRM;
using Marquise_Web.Model.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marquise_Web.Service.IService
{
    public interface IAuthService
    {
        Task<OperationResult<object>> SendOtpAsync(string phoneNumber);
        Task<OperationResult<object>> VerifyOtpAsync(string phoneNumber, string code);
        Task<bool> SignInUserAsync(string userId);
        Task<OperationResult<object>> CheckFailedOtpAttemptsAsync(string phoneNumber);
        Task<List<AccountDto>> GetAccountByUserIdAsync(string userId);
    }
}
