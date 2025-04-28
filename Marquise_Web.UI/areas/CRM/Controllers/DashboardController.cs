using AutoMapper;
using Marquise_Web.UI.areas.CRM.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Marquise_Web.UI.areas.CRM.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        
        public DashboardController()
        {
           
        }

        
        public ActionResult Index()
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            if (claimsPrincipal == null || !claimsPrincipal.HasClaim(c => c.Type == "OtpVerified" && c.Value == "True"))
            {
                return RedirectToAction("SendOtp", "Auth");
            }
            
            return View();
        }
    }
}