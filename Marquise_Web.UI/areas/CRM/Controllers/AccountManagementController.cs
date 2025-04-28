using Marquise_Web.UI.areas.CRM.Models;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Utilities.Map;

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

            
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseData);
                var user = apiResponse?.ResultData?.result?.FirstOrDefault();

                if (user == null)
                return RedirectToAction("Index", "Dashboard");

            var account = UIDataMapper.Mapper.Map<AccountVM>(user);
            return View(account);
                
        }
    }
}