using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marquise_Web.Service.IService
{
    public interface IUpdateService
    {
        Task SyncAccountsToWebsiteAsync();
        Task CleanOldOtpLogsAsync();
        Task CleanOldHangfireJobsAsync();
    }
}
