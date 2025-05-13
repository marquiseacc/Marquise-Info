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

            var fiveMinutesAgo = DateTime.UtcNow.AddMinutes(-5);
            var recentRequests = await unitOfWork.UserRepository.CountRecentOtpRequestsAsync(phoneNumber, fiveMinutesAgo);

            if (recentRequests >= 3)
            {
                var lastRequestTime = await unitOfWork.UserRepository.GetLastOtpRequestTimeAsync(phoneNumber);
                if (lastRequestTime.HasValue)
                {
                    var nextAllowedTime = lastRequestTime.Value.AddMinutes(5);
                    var remaining = nextAllowedTime - DateTime.UtcNow;

                    var minutes = (int)remaining.TotalMinutes;
                    var seconds = remaining.Seconds;

                    var remainingMessage = minutes > 0
                        ? $"{minutes} دقیقه و {seconds} ثانیه"
                        : $"{seconds} ثانیه";

                    return OperationResult<object>.Failure($"تعداد درخواست‌ها بیش از حد مجاز است. لطفاً {remainingMessage} دیگر تلاش کنید.");
                }

                return OperationResult<object>.Failure("تعداد درخواست‌ها بیش از حد مجاز است. لطفاً بعداً تلاش کنید.");
            }

            var otpCode = OtpHelper.GenerateSecureOtp(5);

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("x-api-key", apiSetting.ApiKey);

                var requestModel = new
                {
                    mobile = phoneNumber,
                    templateId = apiSetting.ApiTemplateId,
                    parameters = new[] { new { name = "Code", value = otpCode } }
                };

                var json = JsonSerializer.Serialize(requestModel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    var response = await httpClient.PostAsync(apiSetting.ApiBaseUrl, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        return OperationResult<object>.Failure($"ارسال پیامک با خطا مواجه شد: {errorMessage}");
                    }
                }
                catch (Exception ex)
                {
                    return OperationResult<object>.Failure($"ارسال پیامک با خطا مواجه شد: {ex.Message}");
                }
            }

            user.OtpCode = OtpHelper.HashOtp(otpCode);
            user.OtpExpiration = DateTime.UtcNow.AddMinutes(2);
            await unitOfWork.UserRepository.UpdateAsync(user);

            var requestLog = new OtpRequestLog
            {
                PhoneNumber = phoneNumber,
                RequestTime = DateTime.UtcNow,
                IPAddress = HttpContext.Current?.Request?.UserHostAddress,
                UserId = user.Id
            };

            await unitOfWork.UserRepository.AddOtpRequestLogAsync(requestLog);
            await unitOfWork.UserRepository.SaveAsync();

            return OperationResult<object>.Success(null, "کد با موفقیت ارسال شد.");
        }

        public async Task<OperationResult<object>> VerifyOtpAsync(string phoneNumber, string code)
        {
            var failedAttemptsResult = await CheckFailedOtpAttemptsAsync(phoneNumber);
            if (!failedAttemptsResult.IsSuccess)
                return failedAttemptsResult;

            if (string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(code))
                return OperationResult<object>.Failure("اطلاعات وارد شده ناقص است.");

            var user = await unitOfWork.UserRepository.GetByPhoneNumberAsync(phoneNumber);
            if (user == null || user.OtpCode == null || user.OtpExpiration == null)
                return OperationResult<object>.Failure("اطلاعات وارد شده معتبر نمی‌باشد.");

            if (user.OtpExpiration <= DateTime.UtcNow)
                return OperationResult<object>.Failure("کد ورود منقضی شده است.");

            var isValid = OtpHelper.VerifyOtp(code, user.OtpCode);
            var ip = HttpContext.Current?.Request?.UserHostAddress;

            var verifyLog = new OtpVerifyLog
            {
                PhoneNumber = phoneNumber,
                TryTime = DateTime.UtcNow,
                IPAddress = ip,
                IsSuccess = isValid,
                UserId = user.Id
            };
            await unitOfWork.UserRepository.AddOtpVerifyLogAsync(verifyLog);
            await unitOfWork.UserRepository.SaveAsync();

            if (!isValid)
                return OperationResult<object>.Failure("کد وارد شده معتبر نمی‌باشد.");

            var loginResult = await SignInUserAsync(user.Id);
            if (!loginResult)
                return OperationResult<object>.Failure("خطا در ورود به سیستم.");

            user.OtpCode = null;
            user.OtpExpiration = null;
            await unitOfWork.CompleteAsync();

            return OperationResult<object>.Success(new AuthUserDto
            {
                Id = user.Id,
                PhoneNumber = user.PhoneNumber,
                CRMId = user.CRMId
            }, "ورود با موفقیت انجام شد.");
        }

        public async Task<bool> SignInUserAsync(string userId)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;

            // Claims قبلی + جدید
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.FullName ?? ""),
                new Claim("CRMId", user.CRMId.ToString()),
                new Claim("OtpVerified", "True")
            };

            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            authenticationManager.SignIn(new AuthenticationProperties
            {
                IsPersistent = false
            }, identity);

            return true;
        }

        public async Task<OperationResult<object>> CheckFailedOtpAttemptsAsync(string phoneNumber)
        {
            var fiveMinutesAgo = DateTime.UtcNow.AddMinutes(-5);
            var failedAttempts = await unitOfWork.UserRepository.CountRecentFailedOtpAttemptsAsync(phoneNumber, fiveMinutesAgo);

            if (failedAttempts >= 5)
            {
                var lastFailed = await unitOfWork.UserRepository.GetLastFailedOtpAttemptTimeAsync(phoneNumber);
                if (lastFailed.HasValue)
                {
                    var retryTime = lastFailed.Value.AddMinutes(5);
                    var remaining = retryTime - DateTime.UtcNow;

                    var minutes = (int)remaining.TotalMinutes;
                    var seconds = remaining.Seconds;

                    var msg = minutes > 0
                        ? $"{minutes} دقیقه و {seconds} ثانیه"
                        : $"{seconds} ثانیه";

                    return OperationResult<object>.Failure($"تعداد تلاش‌های ناموفق بیش از حد مجاز است. لطفاً {msg} دیگر تلاش کنید.");
                }

                return OperationResult<object>.Failure("تعداد تلاش‌های ناموفق بیش از حد مجاز است. لطفاً بعداً تلاش کنید.");
            }

            return OperationResult<object>.Success("تعداد تلاش‌ها مجاز است.");
        }

    }

}
