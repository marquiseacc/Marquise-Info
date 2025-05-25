using Marquise_Web.Service.Service;
using Marquise_Web.UI.areas.CRM.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Utilities.Map;

namespace Marquise_Web.UI.areas.CRM.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly UnitOfWorkService unitOfWork;

        public DashboardController(UnitOfWorkService unitOfWork)
        {
            this.unitOfWork = unitOfWork;
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


        public async Task<ActionResult> MainDetail()
        {
            var crmId = ((ClaimsIdentity)User.Identity).FindFirst("CrmAccountId")?.Value;
            var detailVM = new MainDetailVM();

            // support
            var contractDtos = await unitOfWork.ContractService.GetContractsByCrmId(crmId);
            var activeContracts = contractDtos.Where(c => c.status__C == "73bbad0b-6e0f-402b-a581-82a87620dbd7").ToList();
            foreach (var contract in activeContracts)
            {
                if (DateTime.Now >= contract.datestart__C && DateTime.Now <= contract.dateend__C)
                {
                    detailVM.SupportStatus = true;
                    var today = DateTime.Now;
                    detailVM.RestDaies = (contract.dateend__C - today).Days.ToString();
                    continue;
                }
                else detailVM.SupportStatus = false;
            }

            // ticket
            var ticketDtos = await unitOfWork.TicketService.GetTicketsByApplicantIdAsync(crmId);
            var activeTickets = ticketDtos.Where(t => t.Status != "8804f420-0c59-44d2-a4ca-711af8822c56" && t.Status != "9a5e80a8-cc75-46f1-b158-01d58384d4f7").ToList();
            detailVM.ActiveTicketNumber = activeTickets.Count;

            // invoice
            var invoicesDtos = await unitOfWork.InvoiceService.GetInvoicesByAccountIdAsync(crmId);
            var activeInvoice = invoicesDtos.Where(i => i.Status == "defe0fbb-f568-49d8-bd65-f74781635da2" || i.Status == "2190e6ec-d127-48b1-953e-70cc8812e986").ToList();
            detailVM.ActiveInvoiceNumber = activeInvoice.Count;

            // payment
            var paymentDtos = await unitOfWork.InvoiceService.GetPaymentsByAccountIdAsync(crmId);
            PersianCalendar pc = new PersianCalendar();
            int currentShamsiYear = pc.GetYear(DateTime.Now);
            var payments = paymentDtos.Where(p => pc.GetYear(p.PaymentDate) == currentShamsiYear).ToList();
            detailVM.PaymentSum = payments.Sum(p => p.Amount);

            return PartialView("MainDetail", detailVM);
        }

        [HttpGet]
        public async Task<ActionResult> LastTicket()
        {
            var crmId = ((ClaimsIdentity)User.Identity).FindFirst("CrmAccountId")?.Value;

            var ticketDtos = await unitOfWork.TicketService.GetTicketsByApplicantIdAsync(crmId);
            var staffDtos = await unitOfWork.TicketService.GetAllStaffAsync();

            var ticketVMs = UIDataMapper.Mapper.Map<List<TicketVM>>(ticketDtos).Take(3);
            var staffDict = UIDataMapper.Mapper.Map<List<StaffInfo>>(staffDtos).ToDictionary(s => s.UserId, s => s);

            foreach (var ticket in ticketVMs)
            {
                if (!string.IsNullOrEmpty(ticket.ITStaffId) && staffDict.ContainsKey(ticket.ITStaffId))
                {
                    ticket.Staff = staffDict[ticket.ITStaffId];
                }
            }

            return PartialView("LastTicket", ticketVMs);
        }

    }
}