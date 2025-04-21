using System.Web.Mvc;


namespace Marquise_Web.UI.Controllers
{
    public class ContactUsController : Controller
    {

        [Route("ارتباط-با-ما", Name = "Contact")]
        public ActionResult Index()
        {
            ViewData["Form"] = "Contact-Form";
            return View();
        }
    }
}