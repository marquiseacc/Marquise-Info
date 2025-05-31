using Marquise_Web.Model.DTOs.CRM;
using Marquise_Web.Model.Utilities;
using Marquise_Web.Service.Service;
using Marquise_Web.UI.areas.CRM.Models;
using System.Security.Claims;
using System.Threading.Tasks;
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

        [System.Web.Http.Authorize] // فقط کاربران احراز هویت شده با JWT اجازه دارند
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/CRM/AccountApi/UpdateAccount")]
        public async Task<IHttpActionResult> UpdateAccount(CrmAccountVM account)
        {
            var identity = User.Identity as ClaimsIdentity;

            if (identity == null || !identity.IsAuthenticated)
                return Json(new { success = false, message = "دسترسی غیرمجاز" });

            var otpVerified = identity.FindFirst("OtpVerified")?.Value;
            if (otpVerified != "True")
                return Json(new { success = false, message = "کد تایید معتبر نیست" });

            var crmId = identity.FindFirst("CrmAccountId")?.Value;
            var userId = identity.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(crmId))
                return Json(new { success = false, message = "شناسه حساب یافت نشد" });

            var dto = UIDataMapper.Mapper.Map<CrmAccountDto>(account);
            dto.AccountId = crmId;

            var result = await unitOfWork.AccountService.UpdateAccountAsync(dto);

            if (result.IsSuccess)
            {
                // نیازی به SignIn مجدد نیست اگر از JWT استفاده می‌کنید

                return Json(new OperationResult<object>
                {
                    IsSuccess = true,
                    Message = result.Message
                });
            }
            else
            {
                return Json(new OperationResult<object>
                {
                    IsSuccess = false,
                    Message = result.Message
                });
            }
        }
    }
}
