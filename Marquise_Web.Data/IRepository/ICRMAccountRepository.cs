using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marquise_Web.Data.IRepository
{
    public interface ICRMAccountRepository
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByUserNameAsync(string userName);
        Task<User> GetUserByPhoneNumberAsync(string userName);
        Task<bool> UpdatePasswordAsync(User user, string newPassword);
        Task AddUser(User user);
    }
}
