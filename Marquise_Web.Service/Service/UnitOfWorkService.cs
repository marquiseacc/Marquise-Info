using Marquise_Web.Data.IRepository;
using Marquise_Web.Service.IService;
using System.Threading.Tasks;

namespace Marquise_Web.Service.Service
{
    public class UnitOfWorkService : IUnitOfWorkService
    {
        private readonly IUnitOfWorkRepository unitOfWork;
        private readonly ApplicationSignInManager signInManager;

        public IMessageService MessageService { get; }
        public IAuthService AuthService { get; }

        public ITicketService TicketService { get; }

        public IInvoiceService InvoiceService { get; }

        public IQuoteService QuoteService { get; }

        public IAccountService AccountService { get; }

        public IContractService ContractService { get; }

        public IUpdateService UpdateService { get; }

        // سازنده
        public UnitOfWorkService(
            IUnitOfWorkRepository unitOfWork,
            ApplicationSignInManager signInManager, 
            IMessageService messageService,
            IAuthService authService,
            ITicketService ticketService,
            IInvoiceService invoiceService,
            IQuoteService quoteService,
            IAccountService accountService,
            IContractService contractService,
            IUpdateService updateService)
        {
            this.unitOfWork = unitOfWork;
            this.signInManager = signInManager;
            this.MessageService = messageService;
            this.AuthService = authService;
            this.TicketService = ticketService;
            this.InvoiceService = invoiceService;
            this.QuoteService = quoteService;
            this.AccountService = accountService;
            this.ContractService = contractService;
            this.UpdateService = updateService;
        }

        // متد CompleteAsync
        public async Task CompleteAsync()
        {
            await unitOfWork.CompleteAsync();
        }
    }

}
