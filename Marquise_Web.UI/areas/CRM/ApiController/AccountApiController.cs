using AutoMapper;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;


namespace Marquise_Web.UI.Areas.CRM.APIController
{

    public class AccountApiController : ApiController
    {
        //private readonly ICRMAccountService accountService;
        private readonly IMapper mapper;
        private readonly HttpClient httpClient;
        private const string apiUrl = "http://192.168.0.20/api/v1/";
        private const string bearerToken = "kVcSKGD8aoBfEf2fQ93_Aq3yz9XVEps1nYm72iUYPLkVKUH6CmVLuQn44Vlo0Ex1UnTuBl9VxfRs-cavpafp2cAnbLAFF8gJ7kZ1qRgfzwlfMt9rF-0cjAZHuzA_xfecrmpwvjCoFx4SHnqeZHAGcBRUMb_JT_a6HsaHFOTMYlKIxSYWd8bjd3IKo7trIcUy8mhLLnYnohidcln8MqN2VkmwJUt_CyfTfqnRe8LyWyjbMxoFdQ30Y9BgwHVi-2wL7x8UFhmNoBEyyDUjro3aLGmFRCcG_2uJNqCmA-6qZ5RYe9Jfj-GIIiba3ZP2O-xFBuNquBZG6I_d06rodrx7imHNRcLCqtgOIAOchqjuO7O2ykYCk_O2fUnDX0E1P5yo5EtNkmvlTt1c5qqmzj9AwcHEIsePIRZ7QJneX1o4DssrHyygk7OHVjN0B3vymrY49kVC0PLzgcOC_KjZQiZhJK4Z8MV5uSQC_UD-IhOSKPRFrgVKzhetum4DLMWqme-fab5PVR2AujlIj9XuvGdwvyNojt3F2uFLxRYxqgohZZe_UKFhsUh1q_xxJ-0Rb6YsZM7av0lQX-Iu8vY3ztar_pLdYMv-_rlf3SRZpCGascfymEePBw_v8vjtvhaPkO_G1OcfaLGrKkW3v5lwla0AmqZrSUMz8ukJ82YhCQgJyIks6Y2ziEjk-GQUP7qaMqbCMbc1K7sqYZPjxBAfNKp4ElrgOWO2HDGbfLjwi99qMAdNLx9Z";
        //O-0yO8uiC-Z8f7plgSFm4L3GuBghRkLZPmia69_WEcUoYIllQX3Zz2dB0n4dmIq3Z3XzcWD2uwZJ0JZQgBzzCyDVz0bBFxU0TiJuYBbKCPVN2B82kVBhkxqEoqaoCfcP2hjp2lbXOyaeAdTbaRslXIO018s_SHl6rSEKjVmz-HLOfRhyjxu4zmaNfZ-sVsPWdz-t7KuuYnDpfCDTA68lUp7LoKIwF866PoWIDKK2JHhbA5kGn0iyn0Wd4cLiX9k2ty8MRiYAL9kpbLoP8zsdr-30hKSb6ZVx5x2cqmqRuzCzKg_5ga9wlxiax_S7scKOUqPg86sUR64uc0rCzDC0ilb8RhrcWoUjhbN6WaPhr_8iYb2P-bAu66H2Yk8Z-SjgDAppQ0hU02NTupd4jPfFoeKsRSwcFJgV6lIK1kV--MUpFSIMAo3_KWI_phisOL1X8V_J1dYaN-I5JJgausKZfbciEwgi4eV3kY-VyVg4A70Hgy5XUV3UZ4xWr3F3sPAWObqCP9CO02HPa6au3GENoybSMhUi3Ubp4J1BctxSzHItGk-Ep6Ulzw2ZLyNvQXdQzyNeZdPynctp63NhvbB2YO48EaUAi81um7CNSCFJifspAnvqxQv7Eyg-quxbgdYtLioSIdegj-QvbWDnDpaKpVyzTJhqIv0m9TcKYOIoVnMn8JGJwaj58udcijMNbAgOMVS0ND-0HE4rS0DQ8Z5DeI-WcNWy-hJdcC1S4whdjiWgGFO5
        public AccountApiController(
            //ICRMAccountService accountService,
            IMapper mapper, HttpClient httpClient)
        {
            //this.accountService = accountService;
            this.mapper = mapper;
            this.httpClient = httpClient;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/CRM/AccountApi/Login")]
        public async Task<IHttpActionResult> Login()
        {
            ////if (!ModelState.IsValid) return Json(new { success = false, message = "Model state is invalid" });

            ////var UserModel = await accountService.LoginAsync(loginViewModel.UserName, loginViewModel.Password);
            ////if (UserModel == null) return Json(new { success = false, message = "Invalid credentials" });


            ////var CRMId = UserModel.CRMId.ToString();
            ////var CRMSection = "CRM_Account/";
            ////httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            ////var response = await httpClient.GetAsync(apiUrl + CRMSection + CRMId);
            ////if (!response.IsSuccessStatusCode) return Json(new { success = false, message = "Failed to fetch data from API" });


            ////var responseData = await response.Content.ReadAsStringAsync();
            ////var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseData);
            ////var resultModel = apiResponse?.ResultData?.result?.FirstOrDefault();
            ////if (resultModel == null) return Json(new { success = false, message = "No data found in API response" });
            ////var urlHelper = new System.Web.Mvc.UrlHelper();


            //string redirectUrl = urlHelper.Action("Index", "Dashboard", new { resultData = resultModel.ToString() });

            //return Json(new { success = true, redirectUrl });
            return Json(new { success = true });
        }

    }
}