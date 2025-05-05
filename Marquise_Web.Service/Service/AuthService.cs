using Marquise_Web.Data.IRepository;
using Marquise_Web.Service.IService;
using Marquise_Web.Utilities.Messaging;
using System;
using System.Threading.Tasks;
using Marquise_Web.Model.DTOs.CRM;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using System.Web;
using System.Collections.Generic;
using System.Security.Claims;

namespace Marquise_Web.Service.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWorkRepository unitOfWork;
        private readonly ApplicationUserManager userManager;
        private readonly ISmsSender smsSender;

        public AuthService(
            IUnitOfWorkRepository unitOfWork,
            ApplicationUserManager userManager,
            ISmsSender smsSender)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.smsSender = smsSender;
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


        // ارسال کد OTP
        public async Task<bool> SendOtpAsync(string phoneNumber)
        {
            try
            {
                var user = await unitOfWork.UserRepository.GetByPhoneNumberAsync(phoneNumber);
                if (user == null)
                    return false;

                // ایجاد کد OTP
                var otpCode = "123456";
                user.OtpCode = otpCode;
                user.OtpExpiration = DateTime.UtcNow.AddMinutes(2);

                await unitOfWork.CompleteAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            // ارسال پیامک
            //await smsSender.SendSmsAsync(user.PhoneNumber, $"کد ورود شما: {otpCode}");
            return true;
        }

        public async Task<AuthUserDto> VerifyOtpAsync(string phoneNumber, string code)
        {
            var user = await unitOfWork.UserRepository.GetByPhoneNumberAsync(phoneNumber);
            if (user == null || user.OtpCode != code || user.OtpExpiration <= DateTime.UtcNow)
                return null;

            // در اینجا می‌توانید Claim را به Identity اضافه کنید
            var claims = new List<Claim>
            {
                new Claim("OtpVerified", "True") // اضافه کردن claim برای تایید OTP
            };

            var identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            // اضافه کردن claims جدید به Identity
            identity.AddClaims(claims);

            // اضافه کردن claims جدید به Identity
            identity.AddClaims(claims);

            // پاک کردن کد OTP پس از تایید
            user.OtpCode = null;
            user.OtpExpiration = null;

            
            // ذخیره سازی تغییرات و اعمال آنها در دیتابیس
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
