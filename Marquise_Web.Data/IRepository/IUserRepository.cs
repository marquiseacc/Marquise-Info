using System.Threading.Tasks;
using Marquise_Web.Model.Entities;

namespace Marquise_Web.Data.IRepository
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task<ApplicationUser> GetByPhoneNumberAsync(string phoneNumber);
        Task<ApplicationUser> GetByIdAsync(string id);
        Task<ApplicationUser> GetByCRMIdAsync(string crmId);
    }
}
