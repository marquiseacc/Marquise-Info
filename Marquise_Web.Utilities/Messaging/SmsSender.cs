using System;
using System.Threading.Tasks;

namespace Marquise_Web.Utilities.Messaging
{
    public class SmsSender : ISmsSender
    {
        public Task SendSmsAsync(string phoneNumber, string message)
        {
            // برای تست، فقط لاگ بگیر
            Console.WriteLine($"ارسال پیامک به {phoneNumber}: {message}");
            return Task.CompletedTask;
        }
    }
}
