using Marquise_Web.Data.IRepository;
using Marquise_Web.Model.DTOs.CRM;
using Marquise_Web.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;

namespace Marquise_Web.Data.Repository
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository

    {
        private new readonly ApplicationDbContext context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<ApplicationUser> GetByIdAsync(string id)
        {
            return await context.Set<ApplicationUser>()
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<ApplicationUser> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await context.Set<ApplicationUser>()
                .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }
        public async Task<int> CountRecentAsync(string phoneNumber, DateTime since)
        {
            var user = await context.Set<ApplicationUser>()
                .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
            if (user == null)
                return 0;

            return user.OtpRequestLogs.Count(r => r.RequestTime >= since);
        }
        public async Task AddOtpRequestLogAsync(OtpRequestLog log)
        {
            var user = await context.Set<ApplicationUser>()
            .FirstOrDefaultAsync(u => u.PhoneNumber == log.PhoneNumber);
            if (user != null)
            {
                user.OtpRequestLogs.Add(log);
                await context.SaveChangesAsync();
            }
        }
        public async Task AddOtpVerifyLogAsync(OtpVerifyLog log)
        {

            var user = await context.Set<ApplicationUser>()
                .FirstOrDefaultAsync(u => u.PhoneNumber == log.PhoneNumber);
            if (user != null)
            {
                user.OtpVerifyLogs.Add(log);
                await context.SaveChangesAsync();
            }

        }
        public async Task<int> CountRecentFailedOtpAttemptsAsync(string phoneNumber, DateTime since)
        {
            var user = await context.Set<ApplicationUser>()
                .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);

            if (user == null)
                return 0;

            return user.OtpVerifyLogs
                .Count(r => !r.IsSuccess && r.TryTime >= since);
        }
        public async Task<int> CountRecentOtpRequestsAsync(string phoneNumber, DateTime since)
        {
            var user = await context.Set<ApplicationUser>()
                .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);

            if (user == null)
                return 0;

            return user.OtpRequestLogs.Count(r => r.RequestTime >= since);
        }
        public async Task<DateTime?> GetLastOtpRequestTimeAsync(string phoneNumber)
        {
            return await context.OtpRequestLogs
                .Where(x => x.PhoneNumber == phoneNumber)
                .OrderByDescending(x => x.RequestTime)
                .Select(x => (DateTime?)x.RequestTime)
                .FirstOrDefaultAsync();
        }
        public async Task<DateTime?> GetLastFailedOtpAttemptTimeAsync(string phoneNumber)
        {
            return await context.OtpVerifyLogs
                .Where(x => x.PhoneNumber == phoneNumber && !x.IsSuccess)
                .OrderByDescending(x => x.TryTime)
                .Select(x => (DateTime?)x.TryTime)
                .FirstOrDefaultAsync();
        }
        public async Task<List<Account>> GetAccountByUserIdAsync(string userId)
        {
            // مرحله ۱: گرفتن رکوردهایی که UserId == userId
            var directAccounts = await context.Accounts
                .Where(a => a.UserId == userId)
                .ToListAsync();

            // استخراج Id آن‌ها
            var directAccountIds = directAccounts.Select(a => a.Id).ToList();

            // مرحله ۲: گرفتن رکوردهایی که ParentId در لیست بالا باشد
            var childAccounts = await context.Accounts
                .Where(a => directAccountIds.Contains(a.ParentId))
                .ToListAsync();

            // مرحله ۳: ترکیب هر دو لیست و حذف تکراری‌ها
            var allAccounts = directAccounts
                .Concat(childAccounts)
                .GroupBy(a => a.Id)
                .Select(g => g.First())
                .ToList();

            return allAccounts;
        }
        public async Task<Account> GetAccountByCrmAccountIdAsync(string crmAccountId)
        {
            return await context.Accounts.FirstOrDefaultAsync(a => a.CrmAccountId == crmAccountId);
        }
        public async Task UpdateAccount(Account account)
        {
            var existingAccount = await context.Accounts.FindAsync(account.Id);
            if (existingAccount == null)
            {
                return;
            }
            existingAccount.Name = account.Name;
            await context.SaveChangesAsync();
        }
        public async Task<Account> GetByCrmAccountIdAsync(string crmAccountId)
        {
            return await context.Accounts
                .FirstOrDefaultAsync(a => a.CrmAccountId == crmAccountId);
        }

        public async Task AddRangeAsync(List<Account> accounts)
        {
            context.Accounts.AddRange(accounts);
            await context.SaveChangesAsync();

        }


    }

}
