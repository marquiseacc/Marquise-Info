using System.Web.Mvc;

namespace Marquise_Web.UI.Controllers
{
    public class SupportController : Controller
    {
        [Route("پشتیبانی", Name = "Support")]
        public ActionResult Index()
        {
            return View();
        }
    }
}