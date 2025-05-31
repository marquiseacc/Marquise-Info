using Marquise_Web.Service.Service;
using Marquise_Web.UI.areas.CRM.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Marquise_Web.UI.areas.CRM.Controllers
{
    public class AccountController : Controller
    {
        private readonly UnitOfWorkService unitOfWork;

        public AccountController(UnitOfWorkService unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        // GET: CRM/AccountManagement
        public async Task<ActionResult> Index()
        {
            return View();
        }

        public async Task<ActionResult> AccountDetail()
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            if (claimsPrincipal == null || !claimsPrincipal.HasClaim(c => c.Type == "OtpVerified" && c.Value == "True"))
                return RedirectToAction("SendOtp", "Auth");

            var crmId = ((ClaimsIdentity)User.Identity).FindFirst("CrmAccountId")?.Value;

            var accountDto = await unitOfWork.AccountService.GetAccountWithManagerAsync(crmId);
            if (accountDto == null)
                return RedirectToAction("Index", "Dashboard");

            var viewModel = new CrmAccountVM
            {
                AccountId = accountDto.AccountId,
                Name = accountDto.Name,
                Telephone = accountDto.Telephone,
                IndustryCode = accountDto.IndustryCode,
                ShippingAddress = accountDto.ShippingAddress,
                Mobile = accountDto.Mobile,
                shomaremoshtari__C = accountDto.shomaremoshtari__C,
                mahale__C = accountDto.mahale__C,
                cituu__C = accountDto.cituu__C,
                management__C = accountDto.management__C,
                //ManagementName = accountDto.ManagementName
            };

            ViewBag.Industries = Industry.GetAll();
            return View("AccountDetail", viewModel);
        }
    }
}