using Marquise_Web.Service.IService;
using MArquise_Web.Model.DTOs.CRM;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;

namespace Marquise_Web.Service.Service
{
    public class QuoteService: IQuoteService
    {
        private readonly HttpClient httpClient;
        private readonly CRMApiSetting apiSetting;

        public QuoteService(HttpClient httpClient, CRMApiSetting apiSetting)
        {
            this.httpClient = httpClient;
            this.apiSetting = apiSetting;
        }
        public async Task<List<QuoteDto>> GetQuotesByAccountIdAsync(string crmId)
        {
            var section = "CRM_Quote/";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSetting.ApiToken);

            var response = await httpClient.GetAsync(apiSetting.ApiBaseUrl + section);
            if (!response.IsSuccessStatusCode)
                return new List<QuoteDto>();

            var json = await response.Content.ReadAsStringAsync();
            var jObj = JObject.Parse(json);
            var array = jObj["ResultData"]?["result"] as JArray;

            if (array == null)
                return new List<QuoteDto>();

            var filtered = array
                .Where(x => (string)x["AccountId"] == crmId)
                .ToList();

            var serialized = JsonConvert.SerializeObject(filtered);
            var sortedList = JsonConvert
               .DeserializeObject<List<QuoteDto>>(serialized)
               .OrderByDescending(t => t.CreateDate)
               .ToList();
            return sortedList;
        }

        public async Task<QuoteDetailDto> GetQuoteDetailAsync(string quoteId)
        {
            var section = "CRM_Quote/";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSetting.ApiToken);

            var response = await httpClient.GetAsync(apiSetting.ApiBaseUrl + section + quoteId);
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var jObj = JObject.Parse(json);
            var array = jObj["ResultData"]?["result"] as JArray;

            if (array == null || !array.Any())
                return null;

            var firstItem = array.First().ToString();
            return JsonConvert.DeserializeObject<QuoteDetailDto>(firstItem);
        }
    }
}
