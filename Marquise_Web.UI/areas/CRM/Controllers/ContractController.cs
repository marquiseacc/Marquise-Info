using Marquise_Web.UI.areas.CRM.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
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
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> ContractList()
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            if (claimsPrincipal == null || !claimsPrincipal.HasClaim(c => c.Type == "OtpVerified" && c.Value == "True"))
                return new HttpStatusCodeResult(403, "احراز هویت ناقص است");

            var crmId = ((ClaimsIdentity)User.Identity).FindFirst("CrmAccountId")?.Value;
            if (string.IsNullOrEmpty(crmId))
                return new HttpStatusCodeResult(401, "شناسه حساب یافت نشد");

            var contractDtos = await unitOfWork.ContractService.GetContractsByCrmId(crmId);
            var viewModels = UIDataMapper.Mapper.Map<List<ContractVM>>(contractDtos);

            return PartialView("ContractList", viewModels);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Detail(string contractId)
        {
            var contractDto = await unitOfWork.ContractService.GetContractById(contractId);
            if (contractDto == null)
                return new HttpStatusCodeResult(404, "قرارداد یافت نشد");

            var vm = UIDataMapper.Mapper.Map<ContractDetailVm>(contractDto);
            vm.tamdidStatus = vm.tamdidgharardad__C ? "فعال" : "غیرفعال";

            return PartialView("Detail", vm);
        }

    }
}