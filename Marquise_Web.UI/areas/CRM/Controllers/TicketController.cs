using Marquise_Web.UI.areas.CRM.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using MArquise_Web.Model.DTOs.CRM;
using Marquise_Web.Service.IService;
using Utilities.Map;
using Marquise_Web.Service.Service;

namespace Marquise_Web.UI.areas.CRM.Controllers
{
    public class TicketController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly ApiSetting apiSetting;
        private readonly IUnitOfWorkService unitOfWork;

        public TicketController(HttpClient httpClient, ApiSetting apiSetting, IUnitOfWorkService unitOfWork)
        {
            this.httpClient = httpClient;
            this.apiSetting = apiSetting;
            this.unitOfWork = unitOfWork;
        }

        // GET: CRM/Invoice
        public async Task<ActionResult> Index()
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            if (claimsPrincipal == null || !claimsPrincipal.HasClaim(c => c.Type == "OtpVerified" && c.Value == "True"))
            {
                return RedirectToAction("SendOtp", "Auth");
            }
            var crmId = ((ClaimsIdentity)User.Identity).FindFirst("CRMId")?.Value;

            var ticketDtos = await unitOfWork.TicketService.GetTicketsByApplicantIdAsync(crmId);
            var staffDtos = await unitOfWork.TicketService.GetAllStaffAsync();

            var ticketVMs = UIDataMapper.Mapper.Map<List<TicketVM>>(ticketDtos);
            var staffDict = UIDataMapper.Mapper.Map<List<StaffInfo>>(staffDtos).ToDictionary(s => s.UserId, s => s);

            foreach (var ticket in ticketVMs)
            {
                if (!string.IsNullOrEmpty(ticket.ITStaffId) && staffDict.ContainsKey(ticket.ITStaffId))
                {
                    ticket.Staff = staffDict[ticket.ITStaffId];
                }
            }

            return View(ticketVMs);
        }

        [HttpGet]
        [System.Web.Http.Route("CRM/Ticket/Detail")]
        public async Task<ActionResult> Detail(string ticketId)
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            var crmName = claimsPrincipal.Identity.Name;

            // دریافت داده از سرویس
            var ticketDto = await unitOfWork.TicketService.GetTicketByIdAsync(ticketId);
            if (ticketDto == null)
                return RedirectToAction("Index", "Dashboard");

            var answersDto = await unitOfWork.TicketService.GetAnswersByTicketIdAsync(ticketId);
            var staffDtos = await unitOfWork.TicketService.GetAllStaffAsync();

            // مپ DTO ها به ViewModel ها
            var ticketVM = UIDataMapper.Mapper.Map<TicketDetailVm>(ticketDto);
            var answersVM = UIDataMapper.Mapper.Map<List<ShowAnswerVM>>(answersDto);
            var staffVMs = UIDataMapper.Mapper.Map<List<StaffInfo>>(staffDtos);

            // ساخت دیکشنری برای دسترسی سریع به کارشناس‌ها
            var staffDict = staffVMs.ToDictionary(s => s.UserId, s => s);

            // تخصیص کارشناس اصلی تیکت
            ticketVM.Staff = staffDict.ContainsKey(ticketVM.ITStaffId)
                ? staffDict[ticketVM.ITStaffId]
                : null;

            // تخصیص اطلاعات کارشناس به پاسخ‌ها
            foreach (var answer in answersVM)
            {
                answer.Staff = answer.CreateBy != null && staffDict.ContainsKey(answer.CreateBy)
                    ? staffDict[answer.CreateBy]
                    : null;

                answer.StaffName = answer.Staff?.UserId == "9ae2b3e1-056e-4331-8e2f-4930a0d115c0"
                    ? crmName
                    : $"{answer.Staff?.FirstName} {answer.Staff?.LastName}";
            }

            ticketVM.Answers = answersVM;
            return View(ticketVM);
        }


        // GET: CRM/Ticket
        public async Task<ActionResult> NewTicket()
        {
            return View();
        }
    }
}