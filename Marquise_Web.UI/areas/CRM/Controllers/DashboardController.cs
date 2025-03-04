using Marquise_Web.Model.CRM;
using Marquise_Web.UI.areas.CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Marquise_Web.UI.areas.CRM.Controllers
{
    public class DashboardController : Controller
    {
        
        public ActionResult Index([FromBody] Result model)
        {
            return View(model);
        }
    }
}