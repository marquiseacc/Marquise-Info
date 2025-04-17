using System.Threading.Tasks;

namespace Marquise_Web.Utilities.Messaging
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string phoneNumber, string message);
    }
}
