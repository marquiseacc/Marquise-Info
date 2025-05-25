using Marquise_Web.Model.DTOs.CRM;
using Marquise_Web.Model.Utilities;
using Marquise_Web.Service.Service;
using Marquise_Web.UI.areas.CRM.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Utilities.Map;

namespace Marquise_Web.UI.areas.CRM.ApiControllers
{
    public class AccountApiController : ApiController
    {
        private readonly UnitOfWorkService unitOfWork;

        public AccountApiController(UnitOfWorkService unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/CRM/AccountApi/UpdateAccount")]
        public async Task<IHttpActionResult> UpdateAccount(AccountVM account)
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            if (claimsPrincipal == null || !claimsPrincipal.HasClaim(c => c.Type == "OtpVerified" && c.Value == "True"))
            {
                return Json(new { success = false });
            }
            var crmId = ((ClaimsIdentity)User.Identity).FindFirst("CrmAccountId")?.Value;
            var dto = UIDataMapper.Mapper.Map<CrmAccountDto>(account);
            dto.AccountId = crmId;

            var result = await unitOfWork.AccountService.UpdateAccountAsync(dto);

            if (result.IsSuccess)
            {
                var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId")?.Value;

                var claims = new List<Claim>
            {
                new Claim("OtpVerified", "True"),
                new Claim("UserId", userId ?? ""),
                new Claim(ClaimTypes.NameIdentifier, crmId ?? ""),
                new Claim(ClaimTypes.Name, account.Name ?? ""),
                new Claim("CrmAccountId", crmId ?? "")
            };

                var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;


                authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                authenticationManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = false
                }, identity);
                return Json(new OperationResult<object>
                {
                    IsSuccess = true,
                    Message = result.Message
                });
            }
            else {
                return Json(new OperationResult<object>
                {
                    IsSuccess = false,
                    Message = result.Message
                });
            }
        }
    }
}
