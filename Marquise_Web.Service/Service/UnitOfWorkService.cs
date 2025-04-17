using Marquise_Web.Data.IRepository;
using Marquise_Web.Service.IService;
using Marquise_Web.Utilities.Messaging;
using System.Threading.Tasks;

namespace Marquise_Web.Service.Service
{
    public class UnitOfWorkService : IUnitOfWorkService
    {
        private readonly IUnitOfWorkRepository unitOfWork;
        private readonly ApplicationSignInManager signInManager; // اصلاح اینجا
        private readonly ISmsSender smsSender;

        // این وابستگی‌ها از DI تزریق می‌شوند
        public IMessageService MessageService { get; }
        public IAuthService AuthService { get; }

        // سازنده
        public UnitOfWorkService(
            IUnitOfWorkRepository unitOfWork,
            ApplicationSignInManager signInManager, // اصلاح اینجا
            ISmsSender smsSender,
            IMessageService messageService,
            IAuthService authService)
        {
            this.unitOfWork = unitOfWork;
            this.signInManager = signInManager;
            this.smsSender = smsSender;
            this.MessageService = messageService;
            this.AuthService = authService;
        }

        // متد CompleteAsync
        public async Task CompleteAsync()
        {
            await unitOfWork.CompleteAsync();
        }
    }

}
