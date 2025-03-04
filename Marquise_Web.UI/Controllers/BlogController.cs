using System.Web.Mvc;

namespace Marquise_Web.UI.Controllers
{
    
    public class BlogController : Controller
    {
        [Route("مقالات", Name = "Blog")]
        public ActionResult Index()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Route("مشاهده-مقاله", Name = "BlogDetail")]
        public ActionResult BlogDetail()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Route("افزودن-مقاله", Name = "AddBlog")]
        public ActionResult AddBlog()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}