using Marquise_Web.Data.IRepository;
using Marquise_Web.Service.IService;
using Marquise_Web.Utilities.Messaging;
using System;
using System.Threading.Tasks;
using Marquise_Web.Model.Entities;
using Marquise_Web.Model.DTOs.CRM;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using System.Web;

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
            var user = await unitOfWork.UserRepository.GetByPhoneNumberAsync(phoneNumber);
            if (user == null)
                return false;

            // ایجاد کد OTP
            var otpCode = "123456";
            user.OtpCode = otpCode;
            user.OtpExpiration = DateTime.UtcNow.AddMinutes(5);

            await unitOfWork.CompleteAsync();

            // ارسال پیامک
            await smsSender.SendSmsAsync(user.PhoneNumber, $"کد ورود شما: {otpCode}");
            return true;
        }

        // تایید کد OTP
        public async Task<AuthUserDto> VerifyOtpAsync(string phoneNumber, string code)
        {
            var user = await unitOfWork.UserRepository.GetByPhoneNumberAsync(phoneNumber);
            if (user == null || user.OtpCode != code || user.OtpExpiration <= DateTime.UtcNow)
                return null;

            // پاک کردن کد OTP پس از تایید
            user.OtpCode = null;
            user.OtpExpiration = null;

            await unitOfWork.CompleteAsync();

            return new AuthUserDto
            {
                Id = user.Id,
                PhoneNumber = user.PhoneNumber
            };
        }

        public async Task<bool> SignInUserAsync(string userId)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            var identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);
            return true;
        }
    }

}
