using AutoMapper;
using Marquise_Web.Model.CRM;
using Marquise_Web.Service.IService;
using MArquise_Web.Model.CRM;
using Marquise_Web.UI.areas.CRM.Models;
using System.Threading.Tasks;
using System.Web.Http;
using System;
using System.Web.Mvc;
using RouteAttribute = System.Web.Http.RouteAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace Marquise_Web.UI.Areas.CRM.Controllers
{
    public class AccountController : Controller
    {
        
        public AccountController()
        {
            
        }

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ForgetPassword()
        {
            return View();
        }

    }
}