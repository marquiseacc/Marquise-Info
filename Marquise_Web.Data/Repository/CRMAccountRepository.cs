using Marquise_Web.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marquise_Web.Data.Repository
{
    public class CRMAccountRepository: ICRMAccountRepository
    {
        private readonly CRMEntities context;

        public CRMAccountRepository(CRMEntities context)
        {
            this.context = context;
        }

        public async Task AddUser(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public Task<User> GetUserByPhoneNumberAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await context.Users.FirstOrDefaultAsync(u =>u.UserName == userName);
        }

        public async Task<bool> UpdatePasswordAsync(User user, string newPassword)
        {
            var userCheck = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (userCheck != null)
            {
                userCheck.Password = newPassword;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
