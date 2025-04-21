using AutoMapper;
using Marquise_Web.UI.areas.CRM.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Marquise_Web.UI.areas.CRM.Controllers
{
    public class DashboardController : Controller
    {
        private readonly HttpClient httpClient;
        private const string apiUrl = "http://192.168.0.20/api/v1/";
        private const string bearerToken = "O-0yO8uiC-Z8f7plgSFm4L3GuBghRkLZPmia69_WEcUoYIllQX3Zz2dB0n4dmIq3Z3XzcWD2uwZJ0JZQgBzzCyDVz0bBFxU0TiJuYBbKCPVN2B82kVBhkxqEoqaoCfcP2hjp2lbXOyaeAdTbaRslXIO018s_SHl6rSEKjVmz-HLOfRhyjxu4zmaNfZ-sVsPWdz-t7KuuYnDpfCDTA68lUp7LoKIwF866PoWIDKK2JHhbA5kGn0iyn0Wd4cLiX9k2ty8MRiYAL9kpbLoP8zsdr-30hKSb6ZVx5x2cqmqRuzCzKg_5ga9wlxiax_S7scKOUqPg86sUR64uc0rCzDC0ilb8RhrcWoUjhbN6WaPhr_8iYb2P-bAu66H2Yk8Z-SjgDAppQ0hU02NTupd4jPfFoeKsRSwcFJgV6lIK1kV--MUpFSIMAo3_KWI_phisOL1X8V_J1dYaN-I5JJgausKZfbciEwgi4eV3kY-VyVg4A70Hgy5XUV3UZ4xWr3F3sPAWObqCP9CO02HPa6au3GENoybSMhUi3Ubp4J1BctxSzHItGk-Ep6Ulzw2ZLyNvQXdQzyNeZdPynctp63NhvbB2YO48EaUAi81um7CNSCFJifspAnvqxQv7Eyg-quxbgdYtLioSIdegj-QvbWDnDpaKpVyzTJhqIv0m9TcKYOIoVnMn8JGJwaj58udcijMNbAgOMVS0ND-0HE4rS0DQ8Z5DeI-WcNWy-hJdcC1S4whdjiWgGFO5";

        public DashboardController(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ActionResult> Index()
        {
            //    if (!ModelState.IsValid)
            //        return Json(new { success = false, message = "Model state is invalid" });

            //    var CRMSection = "CRM_Contact/";
            //    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            //    var response = await httpClient.GetAsync(apiUrl + CRMSection + CrmCode.CrmId.ToString());
            //    if (!response.IsSuccessStatusCode)
            //        return Json(new { success = false, message = "Failed to fetch data from API" });

            //    var responseData = await response.Content.ReadAsStringAsync();

            //    try
            //    {
            //        var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseData);
            //        var user = apiResponse?.ResultData?.result?.FirstOrDefault();

            //        if (user == null)
            //            return Json(new { success = false, message = "No data found in API response" });

            //        // فقط جهت تست
            //        return Json(new
            //        {
            //            success = true,
            //            fullName = user.FullName,
            //            phone = user.MobilePhone,
            //            gender = user.jensiat__C
            //        });
            //        // ادامه کد...
            //    }
            //    catch (Exception ex)
            //    {
            //        return Json(new { success = false, message = "Error deserializing JSON", details = ex.Message });
            //    }

            return View();
        }

    }
}