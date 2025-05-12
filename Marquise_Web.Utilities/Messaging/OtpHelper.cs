using System;
using System.Security.Cryptography;
using System.Text;

namespace Marquise_Web.Utilities.Messaging
{
    public static class OtpHelper
    {
        public static string GenerateSecureOtp(int length = 5)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] randomNumber = new byte[4];
                rng.GetBytes(randomNumber);
                int otp = Math.Abs(BitConverter.ToInt32(randomNumber, 0)) % (int)Math.Pow(10, length);
                return otp.ToString("D" + length);
            }
        }

        public static string HashOtp(string otp)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(otp);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public static bool VerifyOtp(string enteredOtp, string storedHash)
        {
            var enteredHash = HashOtp(enteredOtp);
            return storedHash == enteredHash;
        }
    }
}
