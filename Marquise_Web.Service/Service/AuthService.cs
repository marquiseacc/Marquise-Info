using Marquise_Web.Data.IRepository;
using Marquise_Web.Model.DTOs.CRM;
using Marquise_Web.Model.Entities;
using Marquise_Web.Model.Utilities;
using Marquise_Web.Service.IService;
using Marquise_Web.Utilities.Messaging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Utilities.Map;

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
                return OperationResult<object>.Failure("کاربر با این شماره یافت نشد.برای پیوستن به تیم مارکیز از طریق صفحه ارتباط با ما اقدام کنید.");

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

            //using (var httpClient = new HttpClient())
            //{
            //    httpClient.DefaultRequestHeaders.Add("x-api-key", apiSetting.ApiKey);

            //    var requestModel = new
            //    {
            //        mobile = phoneNumber,
            //        templateId = apiSetting.ApiTemplateId,
            //        parameters = new[] { new { name = "Code", value = otpCode } }
            //    };

            //    var json = JsonSerializer.Serialize(requestModel);
            //    var content = new StringContent(json, Encoding.UTF8, "application/json");

            //    var response = await httpClient.PostAsync(apiSetting.ApiBaseUrl, content);

            //    if (!response.IsSuccessStatusCode)
            //    {
            //        var errorMessage = await response.Content.ReadAsStringAsync();
            //        return OperationResult<object>.Failure($"ارسال پیامک با خطا مواجه شد: {errorMessage}");
            //    }

            //}

            user.OtpCode = OtpHelper.HashOtp(otpCode);
            user.OtpExpiration = DateTime.UtcNow.AddMinutes(2);
            await unitOfWork.UserRepository.UpdateAsync(user);

            var requestLog = new OtpRequestLog
            {
                Id = Guid.NewGuid().ToString(),
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
                Id = Guid.NewGuid().ToString(),
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

            var jwtToken = await SignInUserAsync(user.Id);
            if (string.IsNullOrEmpty(jwtToken))
                return OperationResult<object>.Failure("خطا در ورود به سیستم.");

            user.OtpCode = null;
            user.OtpExpiration = null;
            await unitOfWork.CompleteAsync();

            return OperationResult<object>.Success(new
            {
                Token = jwtToken,
                User = new AuthUserDto
                {
                    Id = user.Id,
                    PhoneNumber = user.PhoneNumber
                }
            }, "ورود با موفقیت انجام شد.");
        }

        public async Task<string> SignInUserAsync(string userId)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
                return null;

            var claims = new List<Claim>
    {
        new Claim("UserId", user.Id)
    };

            var secretKey = "ThisIsA32CharLongSecretKeyForHS256!!"; // move to config in production
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "MarquiseSupport",
                audience: "MarquiseSupport",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
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

        public async Task<List<AccountDto>> GetAccountByUserIdAsync(string userId)
        {
            var accounts = DataMapper.Mapper.Map<List<AccountDto>>(await unitOfWork.UserRepository.GetAccountByUserIdAsync(userId));
            return accounts;
        }
    }

}
