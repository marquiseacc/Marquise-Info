using Marquise_Web.UI.areas.CRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using MArquise_Web.Model.DTOs.CRM;
using Marquise_Web.Service.Service;
using Utilities.Map;

namespace Marquise_Web.UI.areas.CRM.Controllers
{
    public class ContractController : Controller
    {
        private readonly UnitOfWorkService unitOfWork;

        public ContractController(UnitOfWorkService unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<ActionResult> Index()
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            if (claimsPrincipal == null || !claimsPrincipal.HasClaim(c => c.Type == "OtpVerified" && c.Value == "True"))
                return RedirectToAction("SendOtp", "Auth");

            var crmId = ((ClaimsIdentity)User.Identity).FindFirst("CRMId")?.Value;
            var contractDtos = await unitOfWork.ContractService.GetContractsByCrmId(crmId);
            var viewModels = UIDataMapper.Mapper.Map<List<ContractVM>>(contractDtos);

            return View(viewModels);
        }

        [HttpGet]
        public async Task<ActionResult> Detail(string contractId)
        {
            var contractDto = await unitOfWork.ContractService.GetContractById(contractId);
            if (contractDto == null)
                return RedirectToAction("Index");

            var vm = UIDataMapper.Mapper.Map<ContractDetailVm>(contractDto);
            vm.tamdidStatus = vm.tamdidgharardad__C ? "فعال" : "غیرفعال";

            return PartialView("Detail", vm);
        }
    }
}