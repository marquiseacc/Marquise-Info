﻿using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Marquise_Web.Utilities.convert
{
    public static class PasswordHelper
    {
        public static Task<string> HashPasswordAsync(string password)
        {
            return Task.Run(() =>
            {
                using (var sha256 = SHA256.Create())
                {
                    var bytes = Encoding.UTF8.GetBytes(password);
                    var hash = sha256.ComputeHash(bytes);
                    return Convert.ToBase64String(hash).Trim();  // حذف فضای اضافی در انتهای هش
                }
            });
        }

        public static async Task<bool> VerifyPasswordAsync(string password, string passwordHash)
        {
            var hash = await HashPasswordAsync(password);
            return hash == passwordHash;
        }
    }
}
