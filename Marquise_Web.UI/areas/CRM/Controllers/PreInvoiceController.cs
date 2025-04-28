using Marquise_Web.UI.areas.CRM.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Utilities.Map;

namespace Marquise_Web.UI.areas.CRM.Controllers
{
    public class PreInvoiceController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly ApiSetting apiSetting;

        public PreInvoiceController(HttpClient httpClient, ApiSetting apiSetting)
        {
            this.httpClient = httpClient;
            this.apiSetting = apiSetting;
        }

        public async Task<ActionResult> Index()
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            if (claimsPrincipal == null || !claimsPrincipal.HasClaim(c => c.Type == "OtpVerified" && c.Value == "True"))
            {
                return RedirectToAction("SendOtp", "Auth");
            }
            

            var crmId = ((ClaimsIdentity)User.Identity).FindFirst("CRMId")?.Value;
            var CRMSection = "CRM_Quote/";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSetting.ApiToken);

            var response = await httpClient.GetAsync(apiSetting.ApiBaseUrl + CRMSection);
            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Dashboard");

            var responseString = await response.Content.ReadAsStringAsync();  

            var jObject = JObject.Parse(responseString);                     

            var resultArray = jObject["ResultData"]?["result"] as JArray;      

            if (resultArray == null)
                return RedirectToAction("Index", "Dashboard");

            var filteredRecords = resultArray
                .Where(item => (string)item["AccountId"] == crmId)
                .ToList();

            var filteredJson = JsonConvert.SerializeObject(filteredRecords);
            var quotes = JsonConvert.DeserializeObject<List<QuoteVM>>(filteredJson);

            return View(quotes);
        }

        public async Task<ActionResult> Detail(string quoteId)
        {
            var CRMSection = "CRM_Quote/";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSetting.ApiToken);

            var response = await httpClient.GetAsync(apiSetting.ApiBaseUrl + CRMSection + quoteId);
            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Dashboard");

            var responseString = await response.Content.ReadAsStringAsync();

            var jObject = JObject.Parse(responseString);

            var resultArray = jObject["ResultData"]?["result"] as JArray;

            if (resultArray == null)
                return RedirectToAction("Index", "Dashboard");
            var filteredJson = JsonConvert.SerializeObject(resultArray);
            var quote = JsonConvert.DeserializeObject<QuoteVM>(filteredJson);


            return View(quote);
        }

    }
}