using System.Web.Mvc;

namespace Marquise_Web.UI.Controllers
{
    public class SoftwareController : Controller
    {
        [Route("نرم-افزارها", Name = "Softwares")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("نرم-افزارهای-جانبی", Name = "OtherSoftwares")]
        public ActionResult OtherSoftwares()
        {
            return View();
        }

    }
}