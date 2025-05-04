using Marquise_Web.UI.areas.CRM.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace Marquise_Web.UI.areas.CRM.Controllers
{
    public class TicketController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly ApiSetting apiSetting;

        public TicketController(HttpClient httpClient, ApiSetting apiSetting)
        {
            this.httpClient = httpClient;
            this.apiSetting = apiSetting;
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

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSetting.ApiToken);
            var CRMSection = "Ticket/";
            var response = await httpClient.GetAsync(apiSetting.ApiBaseUrl + CRMSection);
            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Dashboard");

            var responseString = await response.Content.ReadAsStringAsync();

            var jObject = JObject.Parse(responseString);

            var resultArray = jObject["ResultData"]?["result"] as JArray;

            if (resultArray == null)
                return RedirectToAction("Index", "Dashboard");

            var filteredRecords = resultArray
                .Where(item => (string)item["ApplicantId1__C"] == crmId)
                .ToList();

            var filteredJson = JsonConvert.SerializeObject(filteredRecords);
            var tickets = JsonConvert.DeserializeObject<List<TicketVM>>(filteredJson);

            //////////////////
            ///
            var staffResponse = await httpClient.GetAsync(apiSetting.ApiBaseUrl + "users/");
            if (staffResponse.IsSuccessStatusCode)
            {
                var staffJson = await staffResponse.Content.ReadAsStringAsync();
                var staffJObj = JObject.Parse(staffJson);
                var staffArray = staffJObj["ResultData"] as JArray;

                if (staffArray != null)
                {
                    var staffList = JsonConvert.DeserializeObject<List<StaffInfo>>(staffArray.ToString());
                    var staffDict = staffList.ToDictionary(s => s.UserId, s => s); // فرض بر این که StaffInfo شامل StaffId هست

                    foreach (var ticket in tickets)
                    {
                        if (!string.IsNullOrEmpty(ticket.ITStaffId) && staffDict.ContainsKey(ticket.ITStaffId))
                        {
                            ticket.Staff = staffDict[ticket.ITStaffId];
                        }
                    }
                }
            }
            return View(tickets);
        }

        [HttpGet]
        [System.Web.Http.Route("CRM/Ticket/Detail")]
        public async Task<ActionResult> Detail(string ticketId)
         {
            var claimsPrincipal = User as ClaimsPrincipal;
            var CRMSection = "Ticket/";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSetting.ApiToken);

            // دریافت تیکت
            var response = await httpClient.GetAsync(apiSetting.ApiBaseUrl + CRMSection + ticketId);
            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Dashboard");

            var responseString = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(responseString);
            var resultArray = jObject["ResultData"]?["result"] as JArray;


            if (resultArray == null || !resultArray.Any())
                return RedirectToAction("Index", "Dashboard");

            var firstItemJson = resultArray.First().ToString();
            var ticket = JsonConvert.DeserializeObject<TicketDetailVm>(firstItemJson);




            var CRMSection2 = "Tickets/GetTicketBodyWithAttachment/";
            var responseAnswer = await httpClient.GetAsync(apiSetting.ApiBaseUrl + CRMSection2 + ticket.TicketId.ToString());
            if (!responseAnswer.IsSuccessStatusCode)
                return RedirectToAction("Index", "Dashboard");

            var responseStringAnswer = await responseAnswer.Content.ReadAsStringAsync();

            var jObjectAnswer = JObject.Parse(responseStringAnswer);

            var resultArrayAnswer = jObjectAnswer["ResultData"] as JArray;

            if (resultArrayAnswer == null)
                return RedirectToAction("Index", "Dashboard");

            var filteredRecords = resultArrayAnswer.ToList();

            var filteredJson = JsonConvert.SerializeObject(filteredRecords);
            var answers = JsonConvert.DeserializeObject<List<ShowAnswerVM>>(filteredJson);


            // ----------------------------
            // دریافت لیست کارشناسان
            var staffResponse = await httpClient.GetAsync(apiSetting.ApiBaseUrl + "users/");

            var staffJson = await staffResponse.Content.ReadAsStringAsync();
            var staffJObj = JObject.Parse(staffJson);
            var staffArray = staffJObj["ResultData"] as JArray;
            var allStaffs = JsonConvert.DeserializeObject<List<StaffInfo>>(staffArray.ToString());

            ticket.Staff = allStaffs.FirstOrDefault(s => s.UserId == ticket.ITStaffId);


            // ----------------------------
            foreach (var answer in answers)
            {
                answer.Staff = allStaffs.FirstOrDefault(s => s.UserId == answer.CreateBy);
                if (answer.Staff.UserId.ToString() == "9ae2b3e1-056e-4331-8e2f-4930a0d115c0")
                    answer.StaffName = claimsPrincipal.Identity.Name;
                else answer.StaffName = answer.Staff.FirstName + " " + answer.Staff.LastName;
            }

            ticket.Answers = answers;
            return View(ticket);
        }

        // GET: CRM/Ticket
        public async Task<ActionResult> NewTicket()
        {
            return View();
        }
    }
}