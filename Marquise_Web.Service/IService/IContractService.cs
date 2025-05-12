using Marquise_Web.Model.DTOs.CRM;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marquise_Web.Service.IService
{
    public interface IContractService
    {
        Task<List<ContractDto>> GetContractsByCrmId(string crmId);
        Task<ContractDto> GetContractById(string contractId);
    }
}
