using Marquise_Web.Data.IRepository;
using Marquise_Web.Service.IService;
using Marquise_Web.Utilities.Messaging;
using System.Threading.Tasks;

namespace Marquise_Web.Service.Service
{
    public class UnitOfWorkService : IUnitOfWorkService
    {
        private readonly IUnitOfWorkRepository unitOfWork;
        private readonly ApplicationSignInManager signInManager; 
        private readonly ISmsSender smsSender;

        // این وابستگی‌ها از DI تزریق می‌شوند
        public IMessageService MessageService { get; }
        public IAuthService AuthService { get; }

        public ITicketService TicketService { get; }

        public IInvoiceService InvoiceService { get; }

        public IQuoteService QuoteService { get; }

        public IAccountService AccountService { get; }

        public IContractService ContractService { get; }

        // سازنده
        public UnitOfWorkService(
            IUnitOfWorkRepository unitOfWork,
            ApplicationSignInManager signInManager, 
            ISmsSender smsSender,
            IMessageService messageService,
            IAuthService authService,
            ITicketService ticketService,
            IInvoiceService invoiceService,
            IQuoteService quoteService,
            IAccountService accountService,
            IContractService contractService)
        {
            this.unitOfWork = unitOfWork;
            this.signInManager = signInManager;
            this.smsSender = smsSender;
            this.MessageService = messageService;
            this.AuthService = authService;
            this.TicketService = ticketService;
            this.InvoiceService = invoiceService;
            this.QuoteService = quoteService;
            this.AccountService = accountService;
            this.ContractService = contractService;
        }

        // متد CompleteAsync
        public async Task CompleteAsync()
        {
            await unitOfWork.CompleteAsync();
        }
    }

}
