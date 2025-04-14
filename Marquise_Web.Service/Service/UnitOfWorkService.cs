using Marquise_Web.Data.IRepository;
using Marquise_Web.Service.IService;
using System.Threading.Tasks;

namespace Marquise_Web.Service.Service
{
    public class UnitOfWorkService : IUnitOfWorkService
    {
        private readonly IUnitOfWorkRepository unitOfWork;

        public IMessageService MessageService { get; private set; }

        public IAuthService AuthService { get; private set; }

        public UnitOfWorkService(IUnitOfWorkRepository unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            MessageService = new MessageService(unitOfWork);
            AuthService = new AuthService(unitOfWork);
        }

        public async Task CompleteAsync()
        {
            await unitOfWork.CompleteAsync();
        }
    }
}
