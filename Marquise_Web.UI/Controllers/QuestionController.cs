using System.Web.Mvc;

namespace Marquise_Web.UI.Controllers
{
    public class QuestionController : Controller
    {
        [Route("سوالات-متداول", Name = "Questions")]
        public ActionResult Index()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}