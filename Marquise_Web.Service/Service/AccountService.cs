using Marquise_Web.Service.IService;
using Marquise_Web.Model.DTOs.CRM;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Marquise_Web.Data.Repository;
using Marquise_Web.Model.Utilities;

namespace Marquise_Web.Service.Service
{
    public class AccountService: IAccountService
    {
        private readonly HttpClient _httpClient;
        private readonly CRMApiSetting _apiSetting;
        private readonly UnitOfWorkRepository unitOfWork;

        public AccountService(HttpClient httpClient, CRMApiSetting apiSetting, UnitOfWorkRepository unitOfWork)
        {
            _httpClient = httpClient;
            _apiSetting = apiSetting;
            this.unitOfWork = unitOfWork;
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
                //ManagementName = managerName
            };
        }

        public async Task<OperationResult<object>> UpdateAccountAsync(AccountDto account)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiSetting.ApiToken);

            // تبدیل مدل به JSON
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

            var response = await _httpClient.PutAsync(_apiSetting.ApiBaseUrl + "CRM_Account/" + account.AccountId, content);
            var user = await unitOfWork.UserRepository.GetByCRMIdAsync(account.AccountId);
            user.FullName = account.Name;
            await unitOfWork.UserRepository.UpdateAsync(user);
            await unitOfWork.CompleteAsync();
            var user1 = await unitOfWork.UserRepository.GetByCRMIdAsync(account.AccountId);
            if (response.IsSuccessStatusCode) return OperationResult<object>.Success(null, "مشخصات با موفقیت بروزرسانی شد.");
            else return OperationResult<object>.Failure("بروزرسانی با خطا مواجه شد.");
        }
    }
}
