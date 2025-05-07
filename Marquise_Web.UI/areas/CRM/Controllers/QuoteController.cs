using Marquise_Web.Service.Service;
using Marquise_Web.UI.areas.CRM.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Utilities.Map;

namespace Marquise_Web.UI.areas.CRM.Controllers
{
    public class QuoteController : Controller
    {
        private readonly UnitOfWorkService unitOfWork;

        public QuoteController(UnitOfWorkService unitOfWork)
        {
            
            this.unitOfWork = unitOfWork;
        }

        public async Task<ActionResult> Index()
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            if (claimsPrincipal == null || !claimsPrincipal.HasClaim(c => c.Type == "OtpVerified" && c.Value == "True"))
                return RedirectToAction("SendOtp", "Auth");

            var crmId = ((ClaimsIdentity)User.Identity).FindFirst("CRMId")?.Value;
            var quotes = await unitOfWork.QuoteService.GetQuotesByAccountIdAsync(crmId);
            var viewModel = UIDataMapper.Mapper.Map<List<QuoteVM>>(quotes);
            return View(viewModel);
        }

        [HttpGet]
        [System.Web.Http.Route("CRM/PreInvoice/Detail")]
        public async Task<ActionResult> Detail(string quoteId)
        {
            var quote = await unitOfWork.QuoteService.GetQuoteDetailAsync(quoteId);
            if (quote == null)
                return RedirectToAction("Index", "Dashboard");

            var viewModel = UIDataMapper.Mapper.Map<QuoteDetailVm>(quote);
            return PartialView("Detail", viewModel);
        }
    }
}