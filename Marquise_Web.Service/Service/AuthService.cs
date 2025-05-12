using Marquise_Web.Data.IRepository;
using Marquise_Web.Service.IService;
using System;
using System.Threading.Tasks;
using Marquise_Web.Model.DTOs.CRM;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using System.Web;
using System.Collections.Generic;
using System.Security.Claims;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using Marquise_Web.Utilities.Messaging;
using Marquise_Web.Model.Utilities;

namespace Marquise_Web.Service.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWorkRepository unitOfWork;
        private readonly ApplicationUserManager userManager;
        private readonly SMSApiSetting apiSetting;

        public AuthService(
            IUnitOfWorkRepository unitOfWork,
            ApplicationUserManager userManager,
            SMSApiSetting apiSetting)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.apiSetting = apiSetting;
        }

        public ApplicationSignInManager GetSignInManager()
        {
            if (HttpContext.Current != null)
            {
                var context = HttpContext.Current.GetOwinContext();
                if (context != null)
                {
                    var authenticationManager = context.Authentication;
                    return new ApplicationSignInManager(userManager, authenticationManager);
                }
            }
            throw new InvalidOperationException("OwinContext is not available.");
        }


        public async Task<OperationResult<object>> SendOtpAsync(string phoneNumber)
        {
            var user = await unitOfWork.UserRepository.GetByPhoneNumberAsync(phoneNumber);
            if (user == null)
                return OperationResult<object>.Failure("کاربر با این شماره یافت نشد.");

            // چک کردن تعداد درخواست‌های اخیر در ۵ دقیقه گذشته
            var fiveMinutesAgo = DateTime.UtcNow.AddMinutes(-5);
            var recentRequests = await unitOfWork.UserRepository.CountRecentAsync(phoneNumber, fiveMinutesAgo);

            if (recentRequests >= 3)
            {
                return OperationResult<object>.Failure("تعداد درخواست‌های مجاز برای دریافت کد بیش از حد مجاز است.");
            }

            // ایجاد کد OTP
            var otpCode = OtpHelper.GenerateSecureOtp(5);

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("x-api-key", apiSetting.ApiKey);

            var requestModel = new
            {
                mobile = phoneNumber,
                templateId = apiSetting.ApiTemplateId,
                parameters = new[] { new { name = "Code", value = otpCode } }
            };

            var json = JsonSerializer.Serialize(requestModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(apiSetting.ApiBaseUrl, content);
            if (!response.IsSuccessStatusCode)
                return OperationResult<object>.Failure("ارسال پیامک با خطا مواجه شد.");

            // ذخیره OTP هش‌شده
            user.OtpCode = OtpHelper.HashOtp(otpCode);
            user.OtpExpiration = DateTime.UtcNow.AddMinutes(2);
            await unitOfWork.UserRepository.UpdateAsync(user);

            // ثبت لاگ درخواست OTP
            var log = new OtpRequestLog
            {
                PhoneNumber = phoneNumber,
                RequestTime = DateTime.UtcNow,
                IPAddress = HttpContext.Current?.Request?.UserHostAddress, // Optional: IP Address
                UserId = user.Id // اتصال لاگ به کاربر
            };
            await unitOfWork.UserRepository.AddOtpRequestLogAsync(log);
            await unitOfWork.UserRepository.SaveAsync();
            return OperationResult<object>.Success(null, "کد با موفقیت ارسال شد.");
        }


        public async Task<AuthUserDto> VerifyOtpAsync(string phoneNumber, string code)
        {
            var user = await unitOfWork.UserRepository.GetByPhoneNumberAsync(phoneNumber);
            if (user == null || user.OtpExpiration <= DateTime.UtcNow)
                return null;

            if (!OtpHelper.VerifyOtp(code, user.OtpCode))
                return null;

            // OTP صحیح است
            var claims = new List<Claim>
            {
                new Claim("OtpVerified", "True")
            };

            var identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            identity.AddClaims(claims);

            user.OtpCode = null;
            user.OtpExpiration = null;

            await unitOfWork.CompleteAsync();

            return new AuthUserDto
            {
                Id = user.Id,
                PhoneNumber = user.PhoneNumber,
                CRMId = user.CRMId
            };
        }


        public async Task<bool> SignInUserAsync(string userId)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;

            // ابتدا Claims قبلی کاربر را دریافت می‌کنیم
            var existingClaims = await userManager.GetClaimsAsync(user.Id);

            // اضافه کردن Claims جدید به لیست Claims موجود
            var claims = new List<Claim>(existingClaims)
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // شناسه کاربر
                new Claim(ClaimTypes.Name, user.FullName ?? ""), // نام کامل کاربر
                new Claim("CRMId", user.CRMId.ToString()), // شناسه CRM
                new Claim("OtpVerified", "True") // اضافه کردن Claim برای تایید OTP
            };

            // ایجاد ClaimsIdentity جدید با Claims موجود
            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            // انجام عملیات ورود با حذف sessionهای قبلی
            authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);

            return true;
        }


    }

}
