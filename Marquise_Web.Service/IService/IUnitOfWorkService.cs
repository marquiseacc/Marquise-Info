using System.Threading.Tasks;

namespace Marquise_Web.Service.IService
{
    public interface IUnitOfWorkService
    {
        IMessageService MessageService { get; }
        IAuthService AuthService { get; }

        Task CompleteAsync();
    }
}
