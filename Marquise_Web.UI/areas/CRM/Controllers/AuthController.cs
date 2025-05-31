using Marquise_Web.Model.DTOs.CRM;
using Marquise_Web.Model.Utilities;
using Marquise_Web.Service.IService;
using Marquise_Web.Service.Service;
using Marquise_Web.UI.areas.CRM.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
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

        [System.Web.Mvc.HttpPost]
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

        [System.Web.Mvc.HttpGet]
        public ActionResult VerifyOtp()
        {
            var phone = TempData["PhoneNumber"] as string;
            if (string.IsNullOrEmpty(phone))
                return RedirectToAction(nameof(SendOtp));

            TempData.Keep("PhoneNumber"); // تا بعد از رفرش هم باقی بماند

            return View(new VerifyOtpVM { PhoneNumber = phone });
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> VerifyOtp(VerifyOtpVM model)
        {
            if (!ModelState.IsValid)
                return Json(OperationResult<object>.Failure("کد وارد شده معتبر نمی‌باشد."));

            var result = await unitOfWork.AuthService.VerifyOtpAsync(model.PhoneNumber, model.Code);
            if (!result.IsSuccess || result.Data == null)
                return Json(OperationResult<object>.Failure(result.Message ?? "داده نامعتبر است."));

            try
            {
                var jObj = result.Data as JObject ?? JObject.FromObject(result.Data);
                var token = jObj["Token"]?.ToString();
                var user = jObj["User"]?.ToObject<AuthUserDto>();

                if (string.IsNullOrEmpty(token))
                    return Json(OperationResult<object>.Failure("توکن معتبر نیست."));

                return Json(OperationResult<object>.Success(new
                {
                    Token = token,
                    User = user,
                    RedirectUrl = "/CRM/Auth/BranchSelection"
                }, "ورود با موفقیت انجام شد."));
            }
            catch (Exception ex)
            {
                return Json(OperationResult<object>.Failure("خطای داخلی: " + ex.Message));
            }
        }

        [System.Web.Mvc.HttpGet]
        [System.Web.Mvc.Authorize]
        public async Task<JsonResult> GetBranches()
        {
            var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "توکن معتبر نیست یا کاربر شناسایی نشد." }, JsonRequestBehavior.AllowGet);
            }

            var accounts = await unitOfWork.AuthService.GetAccountByUserIdAsync(userId);
            var accountVms = UIDataMapper.Mapper.Map<List<AccountVM>>(accounts);

            return Json(new { success = true, data = accountVms }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult BranchSelection()
        {
            return View(); // View خالی - داده‌ها رو جاوااسکریپت میاره
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.Authorize]
        public ActionResult SetClaims([FromBody] AccountVM accountVM)
        {
            if (accountVM == null)
                return Json(new { success = false, message = "اطلاعات حساب ارسال نشده است." });

            var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId")?.Value ?? "";

            var claims = new List<Claim>
    {
        new Claim("OtpVerified", "True"),
        new Claim("UserId", userId),
        new Claim(JwtRegisteredClaimNames.Sub, accountVM.CrmAccountId ?? ""),
        new Claim(JwtRegisteredClaimNames.UniqueName, accountVM.Name ?? ""),
        new Claim("CrmAccountId", accountVM.CrmAccountId ?? ""),
        new Claim(ClaimTypes.Name, accountVM.Name ?? "")
    };

            var secretKey = "ThisIsA32CharLongSecretKeyForHS256!!"; // باید با استارتاپ یکی باشه
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "MarquiseSupport",
                audience: "MarquiseSupport",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Json(new { success = true, token = tokenString });
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

        [System.Web.Http.AllowAnonymous]
        public ActionResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("SendOtp", "Auth");
        }


    }

}