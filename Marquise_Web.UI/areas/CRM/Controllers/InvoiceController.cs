using Marquise_Web.UI.areas.CRM.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web.Mvc;
using System.Threading.Tasks;
using MArquise_Web.Model.DTOs.CRM;

namespace Marquise_Web.UI.areas.CRM.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly ApiSetting apiSetting;

        public InvoiceController(HttpClient httpClient, ApiSetting apiSetting)
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
            var CRMSection = "WarehouseInvoice/";
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
            var invoice = JsonConvert.DeserializeObject<List<InvoiceVM>>(filteredJson);

            return View(invoice);
        }

        [HttpGet]
        [System.Web.Http.Route("CRM/Invoice/Detail")]
        public async Task<ActionResult> Detail(string invoiceId)
        {
            var CRMSection = "WarehouseInvoice/";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSetting.ApiToken);

            var response = await httpClient.GetAsync(apiSetting.ApiBaseUrl + CRMSection + invoiceId);
            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Dashboard");

            var responseString = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(responseString);
            var resultArray = jObject["ResultData"]?["result"] as JArray;
            if (resultArray == null || !resultArray.Any())
                return RedirectToAction("Index", "Dashboard");

            var firstItemJson = resultArray.First().ToString();
            var invoice = JsonConvert.DeserializeObject<InvoiceDetailVm>(firstItemJson);

            var CRMSection2 = "CRM_Payment/";
            var paymentResponse = await httpClient.GetAsync(apiSetting.ApiBaseUrl + CRMSection2);
            if (!paymentResponse.IsSuccessStatusCode)
                return RedirectToAction("Index", "Dashboard");

            var paymentResponseString = await paymentResponse.Content.ReadAsStringAsync();
            var paymentJObject = JObject.Parse(paymentResponseString);
            var paymentResultArray = paymentJObject["ResultData"]?["result"] as JArray;

            if (paymentResultArray == null)
                return RedirectToAction("Index", "Dashboard");

            var paymentFilteredRecords = paymentResultArray
                .Where(item => (string)item["InvoiceId"] == invoice.InvoiceId)
                .ToList();

            var paymentFilteredJson = JsonConvert.SerializeObject(paymentFilteredRecords);
            var payments = JsonConvert.DeserializeObject<List<PaymentVM>>(paymentFilteredJson);
            invoice.Payments = payments;
            return PartialView("Detail", invoice);
        }

    }
}