using Marquise_Web.Data.IRepository;
using Marquise_Web.Model.DTOs.CRM;
using Marquise_Web.Model.Entities;
using Marquise_Web.Service.IService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Marquise_Web.Service.Service
{
    public class UpdateService : IUpdateService
    {
        private readonly IUnitOfWorkRepository unitOfWork;
        private readonly HttpClient httpClient;
        private readonly CRMApiSetting apiSetting;

        public UpdateService(IUnitOfWorkRepository unitOfWork, HttpClient httpClient, CRMApiSetting apiSetting)
        {
            this.unitOfWork = unitOfWork;
            this.httpClient = httpClient;
            this.apiSetting = apiSetting;
        }
        public async Task SyncAccountsToWebsiteAsync()
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSetting.ApiToken);

            var response = await httpClient.GetAsync(apiSetting.ApiBaseUrl + "CRM_Account");
            if (!response.IsSuccessStatusCode)
                return;

            var responseString = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(responseString);
            var accountsArray = jObject["ResultData"]?["result"] as JArray;

            if (accountsArray == null)
                return;

            // استخراج موبایل‌های معتبر و یکتا
            var uniquePhones = accountsArray
                .Where(a =>
                {
                    try
                    {
                        if (a == null)
                            return false;
                
                        var addToSite = a["AddMarquiseSite__C"] != null && a["AddMarquiseSite__C"].Value<bool>() == true;
                        var isSynced = a["IsSyncedToSite__C"] != null && a["IsSyncedToSite__C"].Value<bool>() == true;
                        var rawMobile = (string)a["Mobile"];
                        var normalized = NormalizeMobile(rawMobile);
                
                        return addToSite && !isSynced && !string.IsNullOrWhiteSpace(normalized);
                    }
                    catch
                    {
                        return false;
                    }
                })
                .Select(a =>
                {
                    try
                    {
                        return NormalizeMobile((string)a["Mobile"]);
                    }
                    catch
                    {
                        return null;
                    }
                })
                .Where(phone => !string.IsNullOrWhiteSpace(phone))
                .Distinct()
                .ToList();


            // نگاشت شماره به کاربر
            var phoneToUserMap = new Dictionary<string, ApplicationUser>();

            foreach (var phone in uniquePhones)
            {
                var user = await unitOfWork.UserRepository.GetByPhoneNumberAsync(phone);
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = phone,
                        PhoneNumber = phone,
                        FullName = ""
                    };

                    try
                    {
                        await unitOfWork.UserRepository.AddAsync(user);
                        await unitOfWork.CompleteAsync();
                    }
                    catch
                    {
                        user = await unitOfWork.UserRepository.GetByPhoneNumberAsync(phone);
                        if (user == null)
                            continue;
                    }
                }
                phoneToUserMap[phone] = user;
            }

            // ساخت یا آپدیت حساب‌ها
            var accountsToAdd = new List<Account>();

            foreach (var accountJson in accountsArray)
            {
                try
                {
                    bool shouldAddToSite = accountJson["AddMarquiseSite__C"] != null &&
                       accountJson["AddMarquiseSite__C"].Type != JTokenType.Null &&
                       accountJson["AddMarquiseSite__C"].Value<bool>();

                    bool isSynced = accountJson["IsSyncedToSite__C"] != null &&
                                    accountJson["IsSyncedToSite__C"].Type != JTokenType.Null &&
                                    accountJson["IsSyncedToSite__C"].Value<bool>();


                    if (!shouldAddToSite || isSynced)
                    {
                        Console.WriteLine("Skip: AddMarquiseSite__C=false یا IsSyncedToSite__C=true");
                        continue;
                    }

                    var rawMobile = (string)accountJson["Mobile"];
                    var mobile = NormalizeMobile(rawMobile);
                    if (string.IsNullOrWhiteSpace(mobile))
                    {
                        Console.WriteLine($"شماره موبایل نامعتبر: {rawMobile}");
                        continue;
                    }

                    var crmAccountId = (string)(accountJson["AccountID"] ?? accountJson["AccountId"]);
                    if (string.IsNullOrWhiteSpace(crmAccountId))
                    {
                        Console.WriteLine("AccountID یا AccountId نال است.");
                        continue;
                    }

                    if (!phoneToUserMap.TryGetValue(mobile, out var user))
                    {
                        Console.WriteLine($"کاربر با موبایل {mobile} یافت نشد.");
                        continue;
                    }

                    var crmParentId = (string)(accountJson["ParentID"] ?? accountJson["ParentId"]);
                    var name = (string)accountJson["Name"];

                    var existingAccount = await unitOfWork.UserRepository.GetAccountByCrmAccountIdAsync(crmAccountId);

                    if (existingAccount == null)
                    {
                        var newAccount = new Account
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = name,
                            CrmAccountId = crmAccountId,
                            CrmParentId = string.IsNullOrWhiteSpace(crmParentId) ? null : crmParentId,
                            UserId = user.Id,
                            ParentId = null
                        };
                        accountsToAdd.Add(newAccount);
                        Console.WriteLine($"اضافه شد: {crmAccountId}");
                    }
                    else
                    {
                        existingAccount.Name = name;
                        existingAccount.CrmParentId = string.IsNullOrWhiteSpace(crmParentId) ? null : crmParentId;
                        existingAccount.UserId = user.Id;
                        Console.WriteLine($"آپدیت شد: {crmAccountId}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"خطا در پردازش اکانت: {ex.Message}");
                }
            }


            // تنظیم ParentId
            var allAccounts = accountsToAdd.ToList();

            foreach (var accountJson in accountsArray)
            {
                var crmAccountId = (string)(accountJson["AccountID"] ?? accountJson["AccountId"]);
                if (string.IsNullOrWhiteSpace(crmAccountId))
                    continue;

                var existingAccount = await unitOfWork.UserRepository.GetAccountByCrmAccountIdAsync(crmAccountId);
                if (existingAccount != null)
                    allAccounts.Add(existingAccount);
            }

            foreach (var account in allAccounts.Where(x => !string.IsNullOrWhiteSpace(x.CrmParentId)))
            {
                var parent = allAccounts.FirstOrDefault(x => x.CrmAccountId == account.CrmParentId)
                          ?? await unitOfWork.UserRepository.GetAccountByCrmAccountIdAsync(account.CrmParentId);

                if (parent != null)
                    account.ParentId = parent.Id;
            }

            // ذخیره نهایی
            if (accountsToAdd.Any())
                await unitOfWork.UserRepository.AddRangeAsync(accountsToAdd);

            await unitOfWork.CompleteAsync();

            // بروزرسانی CRM برای هر حساب جدید
            foreach (var account in accountsToAdd)
            {
                try
                {
                    var jsonContent = JsonConvert.SerializeObject(new
                    {
                        IsSyncedToSite__C = true
                    });

                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var putResponse = await httpClient.PutAsync(apiSetting.ApiBaseUrl + $"CRM_Account/{account.CrmAccountId}", content);

                    if (!putResponse.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"خطا در بروزرسانی CRM برای اکانت {account.CrmAccountId}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"استثناء در بروزرسانی CRM: {ex.Message}");
                }
            }
        }

        // تابع کمکی
        private string NormalizeMobile(string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile))
                return null;

            mobile = mobile.Trim();

            return (mobile.Length == 11 && mobile.StartsWith("09") && mobile.All(char.IsDigit))
                ? mobile.Substring(1)
                : null;
        }

    }
}
