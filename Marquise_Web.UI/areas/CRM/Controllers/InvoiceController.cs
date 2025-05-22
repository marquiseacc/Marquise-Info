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
            var claimsPrincipal = User as ClaimsPrincipal;
            if (claimsPrincipal == null || !claimsPrincipal.HasClaim(c => c.Type == "OtpVerified" && c.Value == "True"))
                return RedirectToAction("SendOtp", "Auth");

            var crmId = ((ClaimsIdentity)User.Identity).FindFirst("CrmAccountId")?.Value;
            var invoicesDtos = await unitOfWork.InvoiceService.GetInvoicesByAccountIdAsync(crmId);
            if (invoicesDtos == null)
                return RedirectToAction("Index", "Dashboard");

            var invoicesVm = UIDataMapper.Mapper.Map<List<InvoiceVM>>(invoicesDtos);
            return View(invoicesVm);
        }

        [HttpGet]
        public async Task<ActionResult> Detail(string invoiceId)
        {
            var invoiceDto = await unitOfWork.InvoiceService.GetInvoiceDetailAsync(invoiceId);
            if (invoiceDto == null)
                return RedirectToAction("Index", "Dashboard");

            var invoiceVm = UIDataMapper.Mapper.Map<InvoiceDetailVm>(invoiceDto);
            return PartialView("Detail", invoiceVm);
        }
    }
}