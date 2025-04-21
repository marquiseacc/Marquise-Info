using Marquise_Web.Data.IRepository;
using Marquise_Web.Model.Entities;
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
            var user = context.Set<ApplicationUser>().ToListAsync();
            return await context.Set<ApplicationUser>()
                .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }
    }
}
