using MArquise_Web.Model.CRM;
using System.Threading.Tasks;

namespace Marquise_Web.Service.IService
{
    public interface ICRMAccountService
    {
        Task<UserModel> LoginAsync(string username, string password);
        Task<bool> UpdatePasswordAsync(UserModel userModel);
    }
}
