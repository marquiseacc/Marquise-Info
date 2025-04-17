using System;
using System.Threading.Tasks;

namespace Marquise_Web.Data.IRepository
{
    public interface IUnitOfWorkRepository : IDisposable
    {
        IMessageRepository MessageRepository { get; }
        IUserRepository UserRepository { get; }
        Task<int> CompleteAsync();
    }
}
