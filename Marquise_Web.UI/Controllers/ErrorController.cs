using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Marquise_Web.UI.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index(int code = 500)
        {
            Response.StatusCode = code;
            ViewBag.StatusCode = code;
            return View("~/Views/Shared/Error.cshtml");
        }

    }
}