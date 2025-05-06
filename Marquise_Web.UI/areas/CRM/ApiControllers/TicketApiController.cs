using Marquise_Web.UI.areas.CRM.Models;
using MArquise_Web.Model.DTOs.CRM;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Marquise_Web.UI.areas.CRM.ApiControllers
{
    public class TicketApiController : ApiController
    {
        private readonly HttpClient httpClient;
        private readonly ApiSetting apiSetting;

        public TicketApiController(HttpClient httpClient, ApiSetting apiSetting)
        {
            this.httpClient = httpClient;
            this.apiSetting = apiSetting;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/CRM/TicketApi/NewTicket")]
        public async Task<IHttpActionResult> NewTicket(NewTicketVM newTicket)
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            if (claimsPrincipal == null || !claimsPrincipal.HasClaim(c => c.Type == "OtpVerified" && c.Value == "True"))
            {
                return Json(new { success = false });
            }

            var crmId = ((ClaimsIdentity)User.Identity).FindFirst("CRMId")?.Value;

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSetting.ApiToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var CRMSection = "Ticket/";

            var requestBody = new
            {
                TemplateAttributeId = "E659E12C-0FAC-49D2-B752-06A8D156CFA8",
                SaveMethod = "fc651d0a-61c2-4c8a-9bec-a1f7a45eb151",
                Status = "28cb76a6-d999-4ee3-887c-66792287453d",
                ApplicantId = "9ae2b3e1-056e-4331-8e2f-4930a0d115c0",
                Title = newTicket.Title,
                ApplicantId1__C = crmId,
                DoneDate = (object)null,
                RecordColor = (object)null,
                LastResponseDate = (object)null,
                ITStaffId = (object)null,
                ProblemManagementId = (object)null,
                AssetId = (object)null,
                IsRemoved = (object)null,
                ITStaffGroupID = (object)null,
                Type = (object)null,
                SatifacationRate = (object)null,
                InfluenceLevel = (object)null,
                GroupId = (object)null,
                Urgency = (object)null,
                DueDate = (object)null,
                Priority = (object)null,
                TicketBody = new
                {
                    TicketBody = new
                    {
                        Body = newTicket.Description,
                        IsPublic = true
                    }
                }
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");


            var response = await httpClient.PostAsync(apiSetting.ApiBaseUrl + CRMSection, content);

            if (!response.IsSuccessStatusCode)
                return Json(new { success = false });
            return Json(new { success = true });
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/CRM/TicketApi/NewAnswer")]
        public async Task<IHttpActionResult> NewAnswer(AnswerVM Answer)
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            if (claimsPrincipal == null || !claimsPrincipal.HasClaim(c => c.Type == "OtpVerified" && c.Value == "True"))
            {
                return Json(new { success = false });
            }

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSetting.ApiToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var CRMSection = "Ticket/";

                string rowId = Answer.TicketId;

                var url = apiSetting.ApiBaseUrl + CRMSection + rowId + "/AddTicketResponse?message=" + Uri.EscapeDataString(Answer.Message);
                var emptyContent = new StringContent("", Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(url, emptyContent);
            
            
            if (!response.IsSuccessStatusCode)
                return Json(new { success = false });
            return Json(new { success = true });
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/CRM/TicketApi/CloseTicket")]
        public async Task<IHttpActionResult> CloseTicket(CloseTicket ticket)
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            if (claimsPrincipal == null || !claimsPrincipal.HasClaim(c => c.Type == "OtpVerified" && c.Value == "True"))
            {
                return Json(new { success = false });
            }

            var crmId = ((ClaimsIdentity)User.Identity).FindFirst("CRMId")?.Value;

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSetting.ApiToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var CRMSection = "Ticket/";
            var requestBody = new
            {
                Status = "9a5e80a8-cc75-46f1-b158-01d58384d4f7"
            };
            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync(apiSetting.ApiBaseUrl + CRMSection + ticket.TicketId, content);
            if (!response.IsSuccessStatusCode)
                return Json(new { success = false });
            return Json(new { success = true });
        }
    }
}
