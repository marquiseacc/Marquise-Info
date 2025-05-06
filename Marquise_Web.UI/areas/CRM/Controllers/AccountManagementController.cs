using Marquise_Web.UI.areas.CRM.Models;
using MArquise_Web.Model.DTOs.CRM;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Marquise_Web.UI.areas.CRM.Controllers
{
    public class AccountManagementController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly ApiSetting apiSetting;

        public AccountManagementController(HttpClient httpClient, ApiSetting apiSetting)
        {
            this.httpClient = httpClient;
            this.apiSetting = apiSetting;
        }
        // GET: CRM/AccountManagement
        public async Task<ActionResult> Index()
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            if (claimsPrincipal == null || !claimsPrincipal.HasClaim(c => c.Type == "OtpVerified" && c.Value == "True"))
            {
                return RedirectToAction("SendOtp", "Auth");
            }
            if (!ModelState.IsValid)
                return RedirectToAction("Index", "Dashboard");

            var crmId = ((ClaimsIdentity)User.Identity).FindFirst("CRMId")?.Value;
            var CRMSection = "CRM_Account/";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSetting.ApiToken);

            var response = await httpClient.GetAsync(apiSetting.ApiBaseUrl + CRMSection + crmId.ToString());
            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Dashboard");

            var responseData = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<AccountApiResponse>(responseData);
            var user = apiResponse?.ResultData?.result?.FirstOrDefault();

            if (user == null)
                return RedirectToAction("Index", "Dashboard");

            var CRMSection2 = "CRM_Contact/";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSetting.ApiToken);

            var contactResponse = await httpClient.GetAsync(apiSetting.ApiBaseUrl + CRMSection2 + user.management__C.ToString());
            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Dashboard");

            var contactResponseData = await contactResponse.Content.ReadAsStringAsync();

            var ContactApiResponse = JsonConvert.DeserializeObject<ContactApiResponse>(contactResponseData);
            var contact = ContactApiResponse?.ResultData?.result?.FirstOrDefault();

            user.ManagementName= contact?.FullName;
            ViewBag.Industries = Industry.GetAll();
            return View(user);

        }
    }
}