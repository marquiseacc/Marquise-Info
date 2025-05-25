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
        public async Task<JsonResult> BranchMenu()
        {
            var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId")?.Value;
            var accounts = await unitOfWork.AuthService.GetAccountByUserIdAsync(userId);
            var accountVms = UIDataMapper.Mapper.Map<List<AccountVM>>(accounts);

            return Json(accountVms, JsonRequestBehavior.AllowGet);
        }

    }
}