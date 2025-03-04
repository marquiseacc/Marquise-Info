using System.Web.Mvc;

namespace Marquise_Web.UI.Controllers
{
    public class OpportunityController : Controller
    {
        [Route("فرصت-شغلی", Name = "Opportunity")]
        public ActionResult Index()
        {
            return View();
        }
    }
}