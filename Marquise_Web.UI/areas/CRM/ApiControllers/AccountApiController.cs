using Marquise_Web.Model.DTOs.CRM;
using Marquise_Web.UI.areas.CRM.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Utilities.Map;
using Marquise_Web.Service.Service;

namespace Marquise_Web.UI.areas.CRM.ApiControllers
{
    public class AccountApiController : ApiController
    {
        private readonly UnitOfWorkService unitOfWork;

        public AccountApiController(UnitOfWorkService unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        [HttpPost]
        [System.Web.Http.Route("api/CRM/AccountApi/UpdateAccount")]
        public async Task<IHttpActionResult> UpdateAccount(AccountVM account)
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            if (claimsPrincipal == null || !claimsPrincipal.HasClaim(c => c.Type == "OtpVerified" && c.Value == "True"))
            {
                return Json(new { success = false });
            }
            var crmId = ((ClaimsIdentity)User.Identity).FindFirst("CRMId")?.Value;
            var dto = UIDataMapper.Mapper.Map<AccountDto>(account);
            dto.AccountId = crmId;

            var result = await unitOfWork.AccountService.UpdateAccountAsync(dto);
            return Json(new { success = result });
        }
    }
}
