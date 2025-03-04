using Marquise_Web.Data;
using Marquise_Web.Data.Repository;
using System;
using System.Web.Mvc;

namespace Marquise_Web.UI.Controllers
{
    public class LandingPageController : Controller
    {
        [Route("")]
        [Route("خانه", Name = "Index")]
        public ActionResult Index()
        {
            return View();
        }
        
    }
}