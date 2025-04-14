using Marquise_Web.Service.IService;
using Marquise_Web.Service.Service;
using Marquise_Web.UI.areas.CRM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Marquise_Web.UI.areas.CRM.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUnitOfWorkService unitOfWork;

        public AuthController(IUnitOfWorkService unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [System.Web.Http.HttpGet]
        public ActionResult SendOtp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendOtp(SendOtpVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await unitOfWork.AuthService.SendOtpAsync(model.PhoneNumber);
            if (!result)
            {
                ModelState.AddModelError("", "کاربر با این شماره یافت نشد.");
                return View(model);
            }

            TempData["PhoneNumber"] = model.PhoneNumber;
            return RedirectToAction(nameof(VerifyOtp));
        }

        [HttpGet]
        public ActionResult VerifyOtp()
        {
            var phone = TempData["PhoneNumber"] as string;
            if (string.IsNullOrEmpty(phone))
                return RedirectToAction(nameof(SendOtp));

            return View(new VerifyOtpVM { PhoneNumber = phone });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyOtp(VerifyOtpVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userDto = await unitOfWork.AuthService.VerifyOtpAsync(model.PhoneNumber, model.Code);
            if (userDto == null)
            {
                ModelState.AddModelError("", "کد اشتباه یا منقضی شده است.");
                return View(model);
            }

            var signInResult = await unitOfWork.AuthService.SignInUserAsync(userDto.Id);
            if (!signInResult)
            {
                ModelState.AddModelError("", "ورود کاربر با خطا مواجه شد.");
                return View(model);
            }

            return RedirectToAction("Index", "Dashboard");
        }
    }
}