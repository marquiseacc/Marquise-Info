using Marquise_Web.Service.IService;
using MArquise_Web.Model.DTOs.CRM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Marquise_Web.Service.Service
{
    public class AccountService: IAccountService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSetting _apiSetting;

        public AccountService(HttpClient httpClient, ApiSetting apiSetting)
        {
            _httpClient = httpClient;
            _apiSetting = apiSetting;
        }

        public async Task<AccountDto> GetAccountWithManagerAsync(string crmId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiSetting.ApiToken);

            var accountResponse = await _httpClient.GetAsync(_apiSetting.ApiBaseUrl + "CRM_Account/" + crmId);
            if (!accountResponse.IsSuccessStatusCode)
                return null;

            var accountJson = await accountResponse.Content.ReadAsStringAsync();
            var accountApiResponse = JsonConvert.DeserializeObject<AccountApiResponse>(accountJson);
            var account = accountApiResponse?.ResultData?.result?.FirstOrDefault();

            if (account == null)
                return null;

            string managerName = null;

            if (!string.IsNullOrEmpty(account.management__C))
            {
                var contactResponse = await _httpClient.GetAsync(_apiSetting.ApiBaseUrl + "CRM_Contact/" + account.management__C);
                if (contactResponse.IsSuccessStatusCode)
                {
                    var contactJson = await contactResponse.Content.ReadAsStringAsync();
                    var contactApiResponse = JsonConvert.DeserializeObject<ContactApiResponse>(contactJson);
                    managerName = contactApiResponse?.ResultData?.result?.FirstOrDefault()?.FullName;
                }
            }

            return new AccountDto
            {
                AccountId = account.AccountId,
                Name = account.Name,
                Telephone = account.Telephone,
                IndustryCode = account.IndustryCode,
                ShippingAddress = account.ShippingAddress,
                Mobile = account.Mobile,
                shomaremoshtari__C = account.shomaremoshtari__C,
                mahale__C = account.mahale__C,
                cituu__C = account.cituu__C,
                management__C = account.management__C,
                ManagementName = managerName
            };
        }
    }
}
