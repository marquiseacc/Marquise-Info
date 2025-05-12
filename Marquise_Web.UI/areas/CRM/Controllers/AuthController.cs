using Marquise_Web.Service.IService;
using Marquise_Web.UI.areas.CRM.Models;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using Marquise_Web.Service.Service;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Marquise_Web.Model.Utilities;

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
        [System.Web.Http.Route("CRM/Auth/SendOtp")]
        public async Task<ActionResult> SendOtp(SendOtpVM model)
        {
                if (!ModelState.IsValid)
                    return Json(new OperationResult<object>
                    {
                        IsSuccess = false,
                        Message = "اطلاعات ورودی نامعتبر است."
                    });

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

                TempData["PhoneNumber"] = model.PhoneNumber;

            return Json(new OperationResult<object>
            {
                IsSuccess = true,
                Message = result.Message,
                Data = new
                {
                    redirectUrl = Url.Action("VerifyOtp", "Auth", new { area = "CRM" })
                }
            });
        }


        [HttpGet]
        public ActionResult VerifyOtp()
        {
            var phone = TempData["PhoneNumber"] as string;
            if (string.IsNullOrEmpty(phone))
                return RedirectToAction(nameof(SendOtp));

            return View(new VerifyOtpVM { PhoneNumber = phone });
        }

        [HttpPost]
        [System.Web.Http.Route("CRM/Auth/VerifyOtp")]
        public async Task<ActionResult> VerifyOtp(VerifyOtpVM model)
        {
            if (!ModelState.IsValid)
            {
                return Json(OperationResult<object>.Failure("ورودی نامعتبر است."));
            }

            var phoneNumber = model.PhoneNumber;
            var userDto = await unitOfWork.AuthService.VerifyOtpAsync(phoneNumber, model.Code);
            if (userDto == null)
            {
                return Json(OperationResult<object>.Failure("کد اشتباه یا منقضی شده است."));
            }

            var signInResult = await unitOfWork.AuthService.SignInUserAsync(userDto.Id.ToString());
            if (!signInResult)
            {
                return Json(OperationResult<object>.Failure("ورود به سیستم با مشکل مواجه شد."));
            }

            // در صورت موفقیت می‌تونی مسیر ریدایرکت رو به صورت داینامیک تعیین کنی
            var redirectUrl = Url.Action("Index", "Dashboard", new { area = "CRM" });

            return Json(OperationResult<object>.Success(
                new { redirectUrl },
                "ورود با موفقیت انجام شد."));
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