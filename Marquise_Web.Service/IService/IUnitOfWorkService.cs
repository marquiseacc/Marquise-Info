using System.Threading.Tasks;

namespace Marquise_Web.Service.IService
{
    public interface IUnitOfWorkService
    {
        IMessageService MessageService { get; }
        IAuthService AuthService { get; }
        ITicketService TicketService { get; }
        IInvoiceService InvoiceService { get; }
        IQuoteService QuoteService { get; }
        IAccountService AccountService { get; }
        IContractService ContractService { get; }   
        Task CompleteAsync();
    }

}
