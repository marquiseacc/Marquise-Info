using Marquise_Web.Service.IService;
using Marquise_Web.Model.DTOs.CRM;
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
        private readonly HttpClient httpClient;
        private readonly CRMApiSetting apiSetting;

        public InvoiceService(HttpClient httpClient, CRMApiSetting apiSetting)
        {
            this.httpClient = httpClient;
            this.apiSetting = apiSetting;
        }

        public async Task<List<InvoiceDto>> GetInvoicesByAccountIdAsync(string accountId)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSetting.ApiToken);
            var response = await httpClient.GetAsync(apiSetting.ApiBaseUrl + "WarehouseInvoice/");
            if (!response.IsSuccessStatusCode) return null;

            var result = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(result);
            var resultArray = jObject["ResultData"]?["result"] as JArray;

            if (resultArray == null) return null;

            var filtered = resultArray
                .Where(x => (string)x["AccountId"] == accountId)
                .ToList();

            var json = JsonConvert.SerializeObject(filtered);
            var sortedList = JsonConvert
               .DeserializeObject<List<InvoiceDto>>(json)
               .OrderByDescending(t => t.CreateDate)
               .ToList();
            return sortedList;
        }

        public async Task<InvoiceDetailDto> GetInvoiceDetailAsync(string invoiceId)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSetting.ApiToken);
            var invoiceResponse = await httpClient.GetAsync(apiSetting.ApiBaseUrl + "WarehouseInvoice/" + invoiceId);
            if (!invoiceResponse.IsSuccessStatusCode) return null;

            var invoiceContent = await invoiceResponse.Content.ReadAsStringAsync();
            var invoiceJObject = JObject.Parse(invoiceContent);
            var invoiceData = invoiceJObject["ResultData"]?["result"]?.FirstOrDefault();
            if (invoiceData == null) return null;

            var invoice = JsonConvert.DeserializeObject<InvoiceDetailDto>(invoiceData.ToString());
            invoice.Payments = await GetPaymentsByInvoiceIdAsync(invoice.InvoiceId);
            return invoice;
        }

        public async Task<List<PaymentDto>> GetPaymentsByInvoiceIdAsync(string invoiceId)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSetting.ApiToken);
            var paymentResponse = await httpClient.GetAsync(apiSetting.ApiBaseUrl + "CRM_Payment/");
            if (!paymentResponse.IsSuccessStatusCode) return null;

            var paymentContent = await paymentResponse.Content.ReadAsStringAsync();
            var paymentJObject = JObject.Parse(paymentContent);
            var paymentArray = paymentJObject["ResultData"]?["result"] as JArray;
            var Payments = new List<PaymentDto>();
            if (paymentArray != null)
            {
                var filtered = paymentArray
                    .Where(x => (string)x["InvoiceId"] == invoiceId)
                    .ToList();
                var json = JsonConvert.SerializeObject(filtered);
                Payments = JsonConvert.DeserializeObject<List<PaymentDto>>(json);
            }
            return Payments;
        }

        public async Task<List<PaymentDto>> GetPaymentsByAccountIdAsync(string accountId)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSetting.ApiToken);
            var paymentResponse = await httpClient.GetAsync(apiSetting.ApiBaseUrl + "CRM_Payment/");
            if (!paymentResponse.IsSuccessStatusCode) return null;

            var paymentContent = await paymentResponse.Content.ReadAsStringAsync();
            var paymentJObject = JObject.Parse(paymentContent);
            var paymentArray = paymentJObject["ResultData"]?["result"] as JArray;
            var Payments = new List<PaymentDto>();
            if (paymentArray != null)
            {
                var filtered = paymentArray
                    .Where(x => (string)x["AccountId"] == accountId)
                    .ToList();
                var json = JsonConvert.SerializeObject(filtered);
                Payments = JsonConvert.DeserializeObject<List<PaymentDto>>(json);
            }
            var sortedList = Payments
               .OrderByDescending(t => t.PaymentDate)
               .ToList();
            return sortedList;
        }
    }
}
