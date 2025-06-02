using Marquise_Web.Data.Repository;
using Marquise_Web.Model.DTOs.CRM;
using Marquise_Web.Model.Utilities;
using Marquise_Web.Service.IService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Marquise_Web.Service.Service
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient httpClient;
        private readonly CRMApiSetting apiSetting;
        private readonly UnitOfWorkRepository unitOfWork;

        public AccountService(HttpClient httpClient, CRMApiSetting apiSetting, UnitOfWorkRepository unitOfWork)
        {
            this.httpClient = httpClient;
            this.apiSetting = apiSetting;
            this.unitOfWork = unitOfWork;
        }

        public async Task<CrmAccountDto> GetAccountWithManagerAsync(string crmId)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSetting.ApiToken);

            var accountResponse = await httpClient.GetAsync(apiSetting.ApiBaseUrl + "CRM_Account/" + crmId);
            if (!accountResponse.IsSuccessStatusCode)
                return null;
            var responseString = await accountResponse.Content.ReadAsStringAsync();
            var accountJson = await accountResponse.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(responseString);
            var resultArray = jObject["ResultData"]?["result"] as JArray;

            var account = JsonConvert.DeserializeObject<CrmAccountDto>(resultArray.First().ToString());

            if (account == null)
                return null;

            return account;
        }
        public async Task<OperationResult<object>> UpdateAccountAsync(CrmAccountDto account)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSetting.ApiToken);

            var jsonContent = JsonConvert.SerializeObject(new
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
                management__C = account.management__C
            });

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync(apiSetting.ApiBaseUrl + "CRM_Account/" + account.AccountId, content);
            var existAccount = await unitOfWork.UserRepository.GetAccountByCrmAccountIdAsync(account.AccountId);
            existAccount.Name = account.Name;
            await unitOfWork.UserRepository.UpdateAccount(existAccount);
            await unitOfWork.UserRepository.SaveAsync();

            

            if (response.IsSuccessStatusCode)
                return OperationResult<object>.Success(null, "مشخصات با موفقیت بروزرسانی شد.");
            else
                return OperationResult<object>.Failure("بروزرسانی با خطا مواجه شد.");
        }
    }
}
