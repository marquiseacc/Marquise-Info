using Marquise_Web.Data.IRepository;
using Marquise_Web.Service.IService;
using Marquise_Web.Utilities.Messaging;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Marquise_Web.Model.Entities;
using Marquise_Web.Model.DTOs.CRM;

namespace Marquise_Web.Service.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWorkRepository unitOfWork;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ISmsSender smsSender;

        public AuthService(IUnitOfWorkRepository unitOfWork, SignInManager<ApplicationUser> signInManager, ISmsSender smsSender)
        {
            this.unitOfWork = unitOfWork;
            this.signInManager = signInManager;
            this.smsSender = smsSender;
        }

        public async Task<bool> SendOtpAsync(string phoneNumber)
        {
            var user = await unitOfWork.UserRepository.GetByPhoneNumberAsync(phoneNumber);
            if (user == null)
                return false;

            var otpCode = new Random().Next(100000, 999999).ToString();
            user.OtpCode = otpCode;
            user.OtpExpiration = DateTime.UtcNow.AddMinutes(5);

            await unitOfWork.CompleteAsync();

            // ارسال پیامک از طریق سرویس Utility
            await smsSender.SendSmsAsync(user.PhoneNumber, $"کد ورود شما: {otpCode}");
            return true;
        }

        // تایید OTP
        public async Task<AuthUserDto> VerifyOtpAsync(string phoneNumber, string code)
        {
            var user = await unitOfWork.UserRepository.GetByPhoneNumberAsync(phoneNumber);
            if (user == null || user.OtpCode != code || user.OtpExpiration <= DateTime.UtcNow)
                return null;

            // پاک کردن کد بعد از استفاده
            user.OtpCode = null;
            user.OtpExpiration = null;

            await unitOfWork.CompleteAsync();

            return new AuthUserDto
            {
                Id = user.Id,
                PhoneNumber = user.PhoneNumber
            };
        }

        // لاگین کاربر
        public async Task<bool> SignInUserAsync(string userId)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            await signInManager.SignInAsync(user, isPersistent: false);
            return true;
        }
    }
}
