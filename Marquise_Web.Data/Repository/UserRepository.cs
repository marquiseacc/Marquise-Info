using Marquise_Web.Data.IRepository;
using Marquise_Web.Model.DTOs.CRM;
using Marquise_Web.Model.Entities;
using System;
using System.Data.Entity;
using System.Linq;
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
        public async Task<ApplicationUser> GetByCRMIdAsync(string crmId)
        {
            Guid crmGuid = Guid.Parse(crmId);
            return await context.Set<ApplicationUser>()
                .FirstOrDefaultAsync(x => x.CRMId == crmGuid);
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

    }
}
