using Marquise_Web.Model.DTOs.CRM;
using Marquise_Web.Model.Utilities;
using Marquise_Web.Service.IService;
using Marquise_Web.Service.Service;
using Marquise_Web.UI.areas.CRM.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Utilities.Map;

namespace Marquise_Web.UI.areas.CRM.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUnitOfWorkService unitOfWork;

        public AuthController(IUnitOfWorkService unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [System.Web.Http.HttpGet]
        public ActionResult SendOtp()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SendOtp(SendOtpVM model)
        {
            if (!ModelState.IsValid)
            {
                return Json(OperationResult<object>.Failure("اطلاعات ورودی نامعتبر است."));
            }

            var phoneNumber = model.PhoneNumber;
            var result = await unitOfWork.AuthService.SendOtpAsync(phoneNumber);

            if (!result.IsSuccess)
            {
                string contactUrl = Url.Action("Index", "ContactUs", new { area = "" });
                return Json(new OperationResult<object>
                {
                    IsSuccess = false,
                    Message = result.Message,
                    Data = new { redirectUrl = contactUrl }
                });
            }

            TempData["PhoneNumber"] = phoneNumber;

            return Json(OperationResult<object>.Success(new
            {
                redirectUrl = Url.Action("VerifyOtp", "Auth", new { area = "CRM" })
            }, result.Message));
        }

        [HttpGet]
        public ActionResult VerifyOtp()
        {
            var phone = TempData["PhoneNumber"] as string;
            if (string.IsNullOrEmpty(phone))
                return RedirectToAction(nameof(SendOtp));

            TempData.Keep("PhoneNumber"); // تا بعد از رفرش هم باقی بماند

            return View(new VerifyOtpVM { PhoneNumber = phone });
        }

        [HttpPost]
        public async Task<ActionResult> VerifyOtp(VerifyOtpVM model)
        {
            if (!ModelState.IsValid)
            {
                return Json(OperationResult<object>.Failure("کد وارد شده معتبر نمی‌باشد."));
            }

            var result = await unitOfWork.AuthService.VerifyOtpAsync(model.PhoneNumber, model.Code);
            if (!result.IsSuccess)
            {
                return Json(OperationResult<object>.Failure(result.Message));
            }

            var userDto = result.Data as AuthUserDto;
            var signInResult = await unitOfWork.AuthService.SignInUserAsync(userDto.Id.ToString());

            if (!signInResult)
            {
                return Json(OperationResult<object>.Failure("ورود به سیستم با مشکل مواجه شد."));
            }
            return Json(OperationResult<object>.Success("ورود با موفقیت انجام شد."));
        }

        public async Task<ActionResult> BranchSelection()
        {
            var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId")?.Value;
            var accounts = await unitOfWork.AuthService.GetAccountByUserIdAsync(userId);
            var accountVms = UIDataMapper.Mapper.Map<List<AccountVM>>(accounts);
            return View(accountVms);
        }

        private ApplicationSignInManager _signInManager;
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        // گرفتن UserManager از OWIN
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [Authorize]
        public ActionResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("SendOtp", "Auth"); // یا هر جایی که باید بعد از خروج برود
        }
    }

}