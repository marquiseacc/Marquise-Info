using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Marquise_Web.UI.areas.CRM.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        
        public DashboardController()
        {
           
        }

        
        public ActionResult Index()
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            if (claimsPrincipal == null || !claimsPrincipal.HasClaim(c => c.Type == "OtpVerified" && c.Value == "True"))
            {
                return RedirectToAction("SendOtp", "Auth");
            }
            
            return View();
        }

        public async Task<ActionResult> MainDetail()
        {
            return PartialView();
        }
        
        public async Task<ActionResult> SupportTimeLine()
        {
            return PartialView();
        }
    }
}