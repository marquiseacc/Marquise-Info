using System.Web.Mvc;

namespace Marquise_Web.UI.Controllers
{
    public class PluginsController : Controller
    {
        [Route("مکمل-ها", Name = "Plugins")]
        public ActionResult Index()
        {
            return View();
        }
    }
}