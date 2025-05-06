using Marquise_Web.Service.IService;
using MArquise_Web.Model.DTOs.CRM;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Marquise_Web.Service.Service
{
    public class TicketService: ITicketService
    {
        private readonly HttpClient httpClient;
        private readonly ApiSetting apiSetting;

        public TicketService(HttpClient httpClient, ApiSetting apiSetting)
        {
            this.httpClient = httpClient;
            this.apiSetting = apiSetting;
        }

        public async Task<List<TicketDto>> GetTicketsByApplicantIdAsync(string crmId)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSetting.ApiToken);
            var response = await httpClient.GetAsync(apiSetting.ApiBaseUrl + "Ticket/");

            if (!response.IsSuccessStatusCode)
                return new List<TicketDto>();

            var responseString = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(responseString);
            var resultArray = jObject["ResultData"]?["result"] as JArray;

            if (resultArray == null) return new List<TicketDto>();

            var filteredRecords = resultArray
                .Where(item => (string)item["ApplicantId1__C"] == crmId)
                .ToList();

            var filteredJson = JsonConvert.SerializeObject(filteredRecords);
            return JsonConvert.DeserializeObject<List<TicketDto>>(filteredJson);
        }

        public async Task<List<StaffDto>> GetAllStaffAsync()
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSetting.ApiToken);
            var response = await httpClient.GetAsync(apiSetting.ApiBaseUrl + "users/");
            if (!response.IsSuccessStatusCode)
                return new List<StaffDto>();

            var responseString = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(responseString);
            var staffArray = jObject["ResultData"] as JArray;

            return staffArray == null
                ? new List<StaffDto>()
                : JsonConvert.DeserializeObject<List<StaffDto>>(staffArray.ToString());
        }

    }
}
