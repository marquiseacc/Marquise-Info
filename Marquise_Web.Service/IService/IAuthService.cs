﻿using Marquise_Web.Model.DTOs.CRM;
using System.Threading.Tasks;

namespace Marquise_Web.Service.IService
{
    public interface IAuthService
    {
        Task<bool> SendOtpAsync(string phoneNumber);
        Task<AuthUserDto> VerifyOtpAsync(string phoneNumber, string code);
        Task<bool> SignInUserAsync(string userId);
    }
}
