using System;
using System.Threading.Tasks;
using Marquise_Web.Model.DTOs.CRM;
using Marquise_Web.Model.Entities;

namespace Marquise_Web.Data.IRepository
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task<ApplicationUser> GetByPhoneNumberAsync(string phoneNumber);
        Task<ApplicationUser> GetByIdAsync(string id);
        Task<ApplicationUser> GetByCRMIdAsync(string crmId);
        Task<int> CountRecentAsync(string phoneNumber, DateTime since);
        Task AddOtpRequestLogAsync(OtpRequestLog log);
        Task AddOtpVerifyLogAsync(OtpVerifyLog log);
        Task<int> CountRecentFailedOtpAttemptsAsync(string phoneNumber, DateTime since);
        Task<int> CountRecentOtpRequestsAsync(string phoneNumber, DateTime since);
        Task<DateTime?> GetLastOtpRequestTimeAsync(string phoneNumber);
        Task<DateTime?> GetLastFailedOtpAttemptTimeAsync(string phoneNumber);

    }
}
