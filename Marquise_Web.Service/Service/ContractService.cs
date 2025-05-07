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
    public class ContractService: IContractService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSetting _apiSetting;

        public ContractService(HttpClient httpClient, ApiSetting apiSetting)
        {
            _httpClient = httpClient;
            _apiSetting = apiSetting;
        }

        public async Task<List<ContractDto>> GetContractsByCrmId(string crmId)
        {
            var url = _apiSetting.ApiBaseUrl + "new_sodoureghofl/";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiSetting.ApiToken);

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return new List<ContractDto>();

            var json = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(json);
            var resultArray = jObject["ResultData"]?["result"] as JArray;

            if (resultArray == null) return new List<ContractDto>();

            var filtered = resultArray
                .Where(x => (string)x["hesab__C"] == crmId)
                .ToList();

            var filteredJson = JsonConvert.SerializeObject(filtered);
            return JsonConvert.DeserializeObject<List<ContractDto>>(filteredJson);
        }

        public async Task<ContractDto> GetContractById(string contractId)
        {
            var url = _apiSetting.ApiBaseUrl + "new_sodoureghofl/" + contractId;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiSetting.ApiToken);

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(json);
            var resultArray = jObject["ResultData"]?["result"] as JArray;

            if (resultArray == null || !resultArray.Any()) return null;

            return JsonConvert.DeserializeObject<ContractDto>(resultArray.First().ToString());
        }
    }
}
