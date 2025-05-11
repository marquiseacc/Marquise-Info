using Marquise_Web.Service.IService;
using Marquise_Web.UI.areas.CRM.Models;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using Marquise_Web.Service.Service;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

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
                    return Json(new { success = false, message = "اطلاعات ورودی نامعتبر است." });

                var phoneNumber = model.PhoneNumber;
                var result = await unitOfWork.AuthService.SendOtpAsync(phoneNumber);
                if (!result)
                {
                    string contactUrl = Url.Action("Index", "ContactUs", new { area = "" });
                    return Json(new
                    {
                        success = false,
                        redirectUrl = contactUrl
                    });
                }

                TempData["PhoneNumber"] = model.PhoneNumber;

            return Json(new
            {
                success = true,
                redirectUrl = Url.Action("VerifyOtp", "Auth", new { area = "CRM" })
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
                return Json(new { success = false, message = "ورودی نامعتبر است." });

             var phoneNumber = model.PhoneNumber;
            var userDto = await unitOfWork.AuthService.VerifyOtpAsync(phoneNumber, model.Code);
            if (userDto == null)
                return Json(new { success = false, message = "کد اشتباه یا منقضی شده است." });

            // فراخوانی متد SignInUserAsync برای ورود کاربر به سیستم
            var signInResult = await unitOfWork.AuthService.SignInUserAsync(userDto.Id.ToString());
            if (!signInResult)
                return Json(new { success = false, message = "ورود به سیستم با مشکل مواجه شد." });


            return Json(new { success = true });
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