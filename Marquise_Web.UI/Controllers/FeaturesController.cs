using System.Web.Mvc;

namespace Marquise_Web.UI.Controllers
{
    public class FeaturesController : Controller
    {
        [Route("ویژگی-مارکیز", Name = "Features")]
        public ActionResult Index()
        {
            return View();
        }
        
    }
}