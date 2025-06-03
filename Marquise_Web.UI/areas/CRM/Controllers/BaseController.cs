using Marquise_Web.Service.Service;
using Marquise_Web.UI.areas.CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Utilities.Map;

namespace Marquise_Web.UI.areas.CRM.Controllers
{
    public class BaseController : Controller
    {
        private readonly UnitOfWorkService unitOfWork;

        public BaseController(UnitOfWorkService unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Authorize] // اطمینان از اینکه JWT بررسی شود
        public async Task<JsonResult> BranchMenu()
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            // بررسی اینکه کلایم وجود دارد و کاربر احراز هویت شده
            if (claimsPrincipal == null || !claimsPrincipal.HasClaim(c => c.Type == "OtpVerified" && c.Value == "True"))
            {
                return Json(new { error = "Unauthorized" }, JsonRequestBehavior.AllowGet);
            }

            var userId = claimsPrincipal.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { error = "UserId is missing" }, JsonRequestBehavior.AllowGet);
            }

            var accounts = await unitOfWork.AuthService.GetAccountByUserIdAsync(userId);
            var accountVms = UIDataMapper.Mapper.Map<List<AccountVM>>(accounts);

            return Json(accountVms, JsonRequestBehavior.AllowGet);
        }
       
    }
}