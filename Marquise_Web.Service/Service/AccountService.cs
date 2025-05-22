using Marquise_Web.Data.Repository;
using Marquise_Web.Model.DTOs.CRM;
using Marquise_Web.Model.Entities;
using Marquise_Web.Model.Utilities;
using Marquise_Web.Service.IService;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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

        public async Task<AccountDto> GetAccountWithManagerAsync(string crmId)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSetting.ApiToken);

            var accountResponse = await httpClient.GetAsync(apiSetting.ApiBaseUrl + "CRM_Account/" + crmId);
            if (!accountResponse.IsSuccessStatusCode)
                return null;
            var responseString = await accountResponse.Content.ReadAsStringAsync();
            var accountJson = await accountResponse.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(responseString);
            var resultArray = jObject["ResultData"]?["result"] as JArray;

            var account = JsonConvert.DeserializeObject<AccountDto>(resultArray.First().ToString());

            if (account == null)
                return null;

            return account;
        }
        public async Task<OperationResult<object>> UpdateAccountAsync(AccountDto account)
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
            //var user = await unitOfWork.UserRepository.GetByCRMIdAsync(account.AccountId);
            //user.FullName = account.Name;
            //await unitOfWork.UserRepository.UpdateAsync(user);
            //await unitOfWork.UserRepository.SaveAsync();

            //// ساخت Claim جدید
            //var claims = new List<Claim>
            //{
            //    new Claim(ClaimTypes.NameIdentifier, user.Id),
            //    new Claim(ClaimTypes.Name, user.FullName ?? ""),
            //    new Claim("CrmUserId", user.CrmUserId.ToString()),
            //    new Claim("OtpVerified", "True")
            //};

            //var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            //// sign-in مجدد با Identity جدید
            //var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            //authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            //authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = true }, identity);

            //if (response.IsSuccessStatusCode)
            //    return OperationResult<object>.Success(null, "مشخصات با موفقیت بروزرسانی شد.");
            //else
            //    return OperationResult<object>.Failure("بروزرسانی با خطا مواجه شد.");
            return OperationResult<object>.Failure("بروزرسانی با خطا مواجه شد.");
        }

        //public async Task SyncAccountsToWebsiteAsync()
        //{
        //    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSetting.ApiToken);

        //    var response = await httpClient.GetAsync(apiSetting.ApiBaseUrl + "CRM_Account");
        //    if (!response.IsSuccessStatusCode)
        //        return;

        //    var responseString = await response.Content.ReadAsStringAsync();
        //    var jObject = JObject.Parse(responseString);
        //    var accountsArray = jObject["ResultData"]?["result"] as JArray;

        //    if (accountsArray == null)
        //        return;

        //    var unsyncedAccounts = accountsArray
        //        .Where(a =>
        //        {
        //            var telephone = (string)a["Telephone"];
        //            var isSyncedToken = a["IsSyncedToSite__C"];
        //            bool isSynced = false;
        //            if (isSyncedToken != null && isSyncedToken.Type == JTokenType.Boolean)
        //                isSynced = (bool)isSyncedToken;

        //            return !string.IsNullOrWhiteSpace(telephone) && !isSynced;
        //        })
        //        .ToList();

        //    var usersToAdd = new List<ApplicationUser>();

        //    foreach (var account in unsyncedAccounts)
        //    {
        //        var accountId = (string)account["AccountId"];

        //        var existingUser = await unitOfWork.UserRepository.GetByCRMIdAsync(accountId);
        //        if (existingUser != null)
        //            continue;

        //        var user = new ApplicationUser
        //        {
        //            Id = Guid.NewGuid().ToString(),
        //            FullName = (string)account["Name"],
        //            PhoneNumber = (string)account["Telephone"],
        //            SecurityStamp = Guid.NewGuid().ToString(),
        //            CrmUserId = Guid.Parse(accountId)
        //        };

        //        usersToAdd.Add(user);
        //    }

        //    if (usersToAdd.Any())
        //    {
        //        await unitOfWork.UserRepository.BulkInsertUsersAsync(usersToAdd);
        //    }

        //    // آپدیت فیلد IsSyncedToSite__C در CRM
        //    foreach (var account in unsyncedAccounts)
        //    {
        //        var accountId = (string)account["AccountId"];
        //        var updateData = new { IsSyncedToSite__C = true };
        //        var content = new StringContent(JsonConvert.SerializeObject(updateData), Encoding.UTF8, "application/json");
        //        await httpClient.PutAsync(apiSetting.ApiBaseUrl + "CRM_Account/" + accountId, content);
        //    }
        //}

    }
}
