using Marquise_Web.Service.Service;
using Marquise_Web.UI.areas.CRM.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
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
            
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> QuoteList()
        {
            var identity = User.Identity as ClaimsIdentity;

            // استخراج مقدار CrmAccountId از کلایم‌های توکن
            var crmId = identity?.FindFirst("CrmAccountId")?.Value;
            var otpVerified = identity?.FindFirst("OtpVerified")?.Value;

            if (string.IsNullOrEmpty(crmId) || otpVerified != "True")
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            // واکشی داده‌ها و بازگشت partial view
            var quotes = await unitOfWork.QuoteService.GetQuotesByAccountIdAsync(crmId);
            var viewModel = UIDataMapper.Mapper.Map<List<QuoteVM>>(quotes);
            return PartialView("QuoteList", viewModel);
        }


        // اصلاح‌شده برای کنترلر MVC
        [HttpGet]
        [Authorize]
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