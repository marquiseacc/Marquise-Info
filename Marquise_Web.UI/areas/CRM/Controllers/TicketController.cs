using Marquise_Web.Model.DTOs.CRM;
using Marquise_Web.Service.IService;
using Marquise_Web.UI.areas.CRM.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Utilities.Map;

namespace Marquise_Web.UI.areas.CRM.Controllers
{
    public class TicketController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly CRMApiSetting apiSetting;
        private readonly IUnitOfWorkService unitOfWork;

        public TicketController(HttpClient httpClient, CRMApiSetting apiSetting, IUnitOfWorkService unitOfWork)
        {
            this.httpClient = httpClient;
            this.apiSetting = apiSetting;
            this.unitOfWork = unitOfWork;
        }

        // GET: CRM/Invoice
        public async Task<ActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> TicketList()
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            if (claimsPrincipal == null || !claimsPrincipal.HasClaim(c => c.Type == "OtpVerified" && c.Value == "True"))
            {
                return new HttpStatusCodeResult(403, "احراز هویت ناقص است");
            }

            var crmId = ((ClaimsIdentity)User.Identity).FindFirst("CrmAccountId")?.Value;
            if (string.IsNullOrEmpty(crmId))
            {
                return new HttpStatusCodeResult(401, "شناسه کاربر معتبر نیست");
            }

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

            return PartialView("TicketList", ticketVMs);
        }


        [HttpGet]
        public ActionResult DetailPage(string ticketId)
        {
            ViewBag.TicketId = ticketId;
            return View(); // این ویو فقط جاوااسکریپت و یک div داره
        }


        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Detail(string ticketId)
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            var crmName = claimsPrincipal.Identity.Name;

            var ticketDto = await unitOfWork.TicketService.GetTicketByIdAsync(ticketId);
            if (ticketDto == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            var answersDto = await unitOfWork.TicketService.GetAnswersByTicketIdAsync(ticketId);
            var staffDtos = await unitOfWork.TicketService.GetAllStaffAsync();

            var ticketVM = UIDataMapper.Mapper.Map<TicketDetailVm>(ticketDto);
            var answersVM = UIDataMapper.Mapper.Map<List<ShowAnswerVM>>(answersDto);
            var staffVMs = UIDataMapper.Mapper.Map<List<StaffInfo>>(staffDtos);

            var staffDict = staffVMs.ToDictionary(s => s.UserId, s => s);

            ticketVM.Staff = staffDict.ContainsKey(ticketVM.ITStaffId)
                ? staffDict[ticketVM.ITStaffId]
                : null;

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

            return PartialView("Detail", ticketVM); // مثلا یه partialView مثل این
        }



        // GET: CRM/Ticket
        public async Task<ActionResult> NewTicket()
        {
            return View();
        }
    }
}