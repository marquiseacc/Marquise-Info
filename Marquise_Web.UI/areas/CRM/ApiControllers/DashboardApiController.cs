using Marquise_Web.Service.Service;
using Marquise_Web.UI.areas.CRM.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Utilities.Convert;

namespace Marquise_Web.UI.areas.CRM.ApiControllers
{
    public class DashboardApiController : ApiController
    {
        private readonly UnitOfWorkService unitOfWork;

        public DashboardApiController(UnitOfWorkService unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/CRM/DashboardApi/SupportTimeLine")]
        public async Task<IHttpActionResult> SupportTimeLine()
        {
            var identity = User.Identity as ClaimsIdentity;
            var crmId = identity?.FindFirst("CrmAccountId")?.Value;

            if (string.IsNullOrEmpty(crmId))
            {
                return Unauthorized(); // 401
            }

            var supportTimes = new List<SupportTimeVM>();

            var contractDtos = await unitOfWork.ContractService.GetContractsByCrmId(crmId);
            var activeContracts = contractDtos
                .Where(c => c.status__C == "73bbad0b-6e0f-402b-a581-82a87620dbd7")
                .ToList();

            foreach (var contract in activeContracts)
            {
                supportTimes.Add(new SupportTimeVM
                {
                    StartDate = contract.datestart__C.ToPersianDateString(),
                    EndDate = contract.dateend__C.ToPersianDateString()
                });
            }

            return Json(new { success = true, data = supportTimes });
        }

    }
}
