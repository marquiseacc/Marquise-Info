using Marquise_Web.UI.areas.CRM.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Mvc;
using System.Threading.Tasks;
using Marquise_Web.Service.Service;
using Utilities.Map;

namespace Marquise_Web.UI.areas.CRM.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly UnitOfWorkService unitOfWork;

        public InvoiceController(UnitOfWorkService unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<ActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> InvoiceList()
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            if (claimsPrincipal == null || !claimsPrincipal.HasClaim(c => c.Type == "OtpVerified" && c.Value == "True"))
                return new HttpStatusCodeResult(403, "احراز هویت ناقص است");

            var crmId = ((ClaimsIdentity)User.Identity).FindFirst("CrmAccountId")?.Value;
            if (string.IsNullOrEmpty(crmId))
                return new HttpStatusCodeResult(401, "شناسه کاربر یافت نشد");

            var invoicesDtos = await unitOfWork.InvoiceService.GetInvoicesByAccountIdAsync(crmId);
            var invoicesVm = UIDataMapper.Mapper.Map<List<InvoiceVM>>(invoicesDtos);
            return PartialView("InvoiceList", invoicesVm);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Detail(string invoiceId)
        {
            var invoiceDto = await unitOfWork.InvoiceService.GetInvoiceDetailAsync(invoiceId);
            if (invoiceDto == null)
                return new HttpStatusCodeResult(404, "صورتحساب یافت نشد");

            var invoiceVm = UIDataMapper.Mapper.Map<InvoiceDetailVm>(invoiceDto);
            return PartialView("Detail", invoiceVm);
        }

    }
}