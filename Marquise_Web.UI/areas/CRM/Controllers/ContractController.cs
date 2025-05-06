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

namespace Marquise_Web.UI.areas.CRM.Controllers
{
    public class ContractController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly ApiSetting apiSetting;

        public ContractController(HttpClient httpClient, ApiSetting apiSetting)
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
            var CRMSection = "new_sodoureghofl/";
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
                .Where(item => (string)item["hesab__C"] == crmId)
                .ToList();

            var filteredJson = JsonConvert.SerializeObject(filteredRecords);
            var contract = JsonConvert.DeserializeObject<List<ContractVM>>(filteredJson);

            return View(contract);
        }

        [HttpGet]
        [System.Web.Http.Route("CRM/Contract/Detail")]
        public async Task<ActionResult> Detail(string contractId)
        {
            var CRMSection = "new_sodoureghofl/";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSetting.ApiToken);

            var response = await httpClient.GetAsync(apiSetting.ApiBaseUrl + CRMSection + contractId);
            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Dashboard");

            var responseString = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(responseString);

            var resultArray = jObject["ResultData"]?["result"] as JArray;

            if (resultArray == null || !resultArray.Any())
                return RedirectToAction("Index", "Dashboard");

            var firstItemJson = resultArray.First().ToString();

            var contract = JsonConvert.DeserializeObject<ContractDetailVm>(firstItemJson);
            if (contract.tamdidgharardad__C) contract.tamdidStatus = "فعال";
            else contract.tamdidStatus = "غیر فعال";

            return PartialView("Detail", contract);
        }

    }
}