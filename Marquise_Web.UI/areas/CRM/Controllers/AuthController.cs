using Marquise_Web.Service.IService;
using Marquise_Web.UI.areas.CRM.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using Marquise_Web.Service.Service;
using Microsoft.AspNet.Identity.Owin;

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

            var result = await unitOfWork.AuthService.SendOtpAsync(model.PhoneNumber);
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
        //[ValidateAntiForgeryToken]
        [System.Web.Http.Route("CRM/Auth/VerifyOtp")]
        public async Task<ActionResult> VerifyOtp(VerifyOtpVM model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "ورودی نامعتبر است." });

            var userDto = await unitOfWork.AuthService.VerifyOtpAsync(model.PhoneNumber, model.Code);
            if (userDto == null)
                return Json(new { success = false, message = "کد اشتباه یا منقضی شده است." });

            var user = await UserManager.FindByIdAsync(userDto.Id.ToString());
            if (user == null)
                return Json(new { success = false, message = "کاربر یافت نشد." });

            // ورود کاربر با Identity
            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

            // ارسال CRMId برای انتقال به داشبورد
            return Json(new { success = true, crmId = userDto.CRMId });
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

    }
}