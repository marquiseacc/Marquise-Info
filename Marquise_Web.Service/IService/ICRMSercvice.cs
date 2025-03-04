using Marquise_Web.Model.CRM;
using System.Threading.Tasks;

namespace Marquise_Web.Service.IService
{
    public interface ICRMSercvice
    {
        Task<CostumerCRMModel> GetCustomerInfoAsync(int customerId);
    }
}
