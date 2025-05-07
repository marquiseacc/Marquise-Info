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
    public class InvoiceService: IInvoiceService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSetting _apiSetting;

        public InvoiceService(HttpClient httpClient, ApiSetting apiSetting)
        {
            _httpClient = httpClient;
            _apiSetting = apiSetting;
        }

        public async Task<List<InvoiceDto>> GetInvoicesByAccountIdAsync(string accountId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiSetting.ApiToken);
            var response = await _httpClient.GetAsync(_apiSetting.ApiBaseUrl + "WarehouseInvoice/");
            if (!response.IsSuccessStatusCode) return null;

            var result = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(result);
            var resultArray = jObject["ResultData"]?["result"] as JArray;

            if (resultArray == null) return null;

            var filtered = resultArray
                .Where(x => (string)x["AccountId"] == accountId)
                .ToList();

            var json = JsonConvert.SerializeObject(filtered);
            return JsonConvert.DeserializeObject<List<InvoiceDto>>(json);
        }

        public async Task<InvoiceDetailDto> GetInvoiceDetailAsync(string invoiceId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiSetting.ApiToken);
            var invoiceResponse = await _httpClient.GetAsync(_apiSetting.ApiBaseUrl + "WarehouseInvoice/" + invoiceId);
            if (!invoiceResponse.IsSuccessStatusCode) return null;

            var invoiceContent = await invoiceResponse.Content.ReadAsStringAsync();
            var invoiceJObject = JObject.Parse(invoiceContent);
            var invoiceData = invoiceJObject["ResultData"]?["result"]?.FirstOrDefault();
            if (invoiceData == null) return null;

            var invoice = JsonConvert.DeserializeObject<InvoiceDetailDto>(invoiceData.ToString());

            var paymentResponse = await _httpClient.GetAsync(_apiSetting.ApiBaseUrl + "CRM_Payment/");
            if (!paymentResponse.IsSuccessStatusCode) return invoice;

            var paymentContent = await paymentResponse.Content.ReadAsStringAsync();
            var paymentJObject = JObject.Parse(paymentContent);
            var paymentArray = paymentJObject["ResultData"]?["result"] as JArray;

            if (paymentArray != null)
            {
                var filtered = paymentArray
                    .Where(x => (string)x["InvoiceId"] == invoice.InvoiceId)
                    .ToList();
                var json = JsonConvert.SerializeObject(filtered);
                invoice.Payments = JsonConvert.DeserializeObject<List<PaymentDto>>(json);
            }

            return invoice;
        }
    }
}
