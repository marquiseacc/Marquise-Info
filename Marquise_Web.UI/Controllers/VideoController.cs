using System.Web.Mvc;

namespace Marquise_Web.UI.Controllers
{
    public class VideoController : Controller
    {
        [Route("ویدئوهای-آموزشی", Name = "Videos")]
        public ActionResult Index()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Route("مشاهده-ویدئو", Name = "VideoDetail")]
        public ActionResult VideoDetail()
        {
            return View();
        }

    }
}