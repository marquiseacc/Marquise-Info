using Marquise_Web.UI.areas.CRM.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                Title = newTicket.Title,
                ApplicantId = "9ae2b3e1-056e-4331-8e2f-4930a0d115c0",
                TemplateAttributeId = "E659E12C-0FAC-49D2-B752-06A8D156CFA8",
                ApplicantId1__C = crmId
            };

            var json = JsonConvert.SerializeObject(requestBody); // نیاز به Newtonsoft.Json
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(apiSetting.ApiBaseUrl + CRMSection, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<RootResponse>(responseContent);

                string rowId = result?.ResultData?.rowId;

                var url = apiSetting.ApiBaseUrl + CRMSection + rowId + "/AddTicketResponse?message=" + Uri.EscapeDataString(newTicket.Description);
                var emptyContent = new StringContent("", Encoding.UTF8, "application/json");

                var messageResponse = await httpClient.PostAsync(url, emptyContent);
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
            }


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

            Answer.TicketId = "c5bebb7f-cc3c-4612-8e56-0a9def3796f5";
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
    }
}
