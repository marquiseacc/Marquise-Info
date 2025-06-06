﻿using Marquise_Web.Service.IService;
using Marquise_Web.Model.DTOs.CRM;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System;
using Marquise_Web.Model.Utilities;

namespace Marquise_Web.Service.Service
{
    public class TicketService : ITicketService
    {
        private readonly HttpClient httpClient;
        private readonly CRMApiSetting apiSetting;

        public TicketService(HttpClient httpClient, CRMApiSetting apiSetting)
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
            var sortedList = JsonConvert
                .DeserializeObject<List<TicketDto>>(filteredJson)
                .OrderByDescending(t => t.CreateDate) 
                .ToList();
            return sortedList;
        }

        public async Task<TicketDetailDto> GetTicketByIdAsync(string ticketId)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSetting.ApiToken);
            var response = await httpClient.GetAsync(apiSetting.ApiBaseUrl + "Ticket/" + ticketId);
            if (!response.IsSuccessStatusCode) return null;

            var responseString = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(responseString);
            var resultArray = jObject["ResultData"]?["result"] as JArray;
            if (resultArray == null || !resultArray.Any()) return null;

            return JsonConvert.DeserializeObject<TicketDetailDto>(resultArray.First().ToString());
        }

        public async Task<List<AnswerDto>> GetAnswersByTicketIdAsync(string ticketId)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSetting.ApiToken);
            var response = await httpClient.GetAsync(apiSetting.ApiBaseUrl + "Tickets/GetTicketBodyWithAttachment/" + ticketId);
            if (!response.IsSuccessStatusCode) return new List<AnswerDto>();

            var responseString = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(responseString);
            var resultArray = jObject["ResultData"] as JArray;
            if (resultArray == null) return new List<AnswerDto>();

            var sortedList = JsonConvert
               .DeserializeObject<List<AnswerDto>>(resultArray.ToString())
               .OrderByDescending(t => t.CreateDate)
               .ToList();
            return sortedList;
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

        public async Task<OperationResult<object>> CreateTicketAsync(NewTicketDto dto)
        {
            var requestBody = new
            {
                TemplateAttributeId = "E659E12C-0FAC-49D2-B752-06A8D156CFA8",
                SaveMethod = "fc651d0a-61c2-4c8a-9bec-a1f7a45eb151",
                Status = "28cb76a6-d999-4ee3-887c-66792287453d",
                ApplicantId = "9ae2b3e1-056e-4331-8e2f-4930a0d115c0",
                Title = dto.Title,
                ApplicantId1__C = dto.CrmId,
                DoneDate = (object)null,
                RecordColor = (object)null,
                LastResponseDate = (object)null,
                ITStaffId = (object)null,
                ProblemManagementId = (object)null,
                AssetId = (object)null,
                IsRemoved = (object)null,
                ITStaffGroupID = (object)null,
                Type = (object)null,
                SatifacationRate = (object)null,
                InfluenceLevel = (object)null,
                GroupId = (object)null,
                Urgency = (object)null,
                DueDate = (object)null,
                Priority = (object)null,
                TicketBody = new
                {
                    TicketBody = new
                    {
                        Body = dto.Description,
                        IsPublic = true
                    }
                }
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSetting.ApiToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.PostAsync(apiSetting.ApiBaseUrl + "Ticket/", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return OperationResult<object>.Success(null, "تیکت با موفقیت ثبت شد.");
            }
            else
            {
                var errorMsg = $"ثبت تیکت با خطا مواجه شد. کد خطا: {response.StatusCode}";
                // می‌تونی responseBody هم log کنی یا تحلیل کنی
                return OperationResult<object>.Failure(errorMsg);
            }

        }

        public async Task<OperationResult<object>> AddAnswerAsync(NewAnswerDto dto)
        {
            var url = $"{apiSetting.ApiBaseUrl}Ticket/{dto.TicketId}/AddTicketResponse?message={Uri.EscapeDataString(dto.Message)}";
            var emptyContent = new StringContent("", Encoding.UTF8, "application/json");

            var statusUrl = $"{apiSetting.ApiBaseUrl}Ticket/{dto.TicketId}";
            var requestBody = new
            {
                Status = "b1af65c3-af6e-48a6-8d1f-b5c09d7f03c9" // وضعیت "بسته شده"
            };
            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSetting.ApiToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.PostAsync(url, emptyContent);
            var statusResponse = await httpClient.PutAsync(statusUrl, content);
            if (response.IsSuccessStatusCode && statusResponse.IsSuccessStatusCode)
            {
                return OperationResult<object>.Success(null, "پاسخ شما با موفقیت ثبت شد.");
            }
            else
            {
                var responseText = await response.Content.ReadAsStringAsync(); // برای دیباگ یا لاگ
                return OperationResult<object>.Failure("ثبت پاسخ با خطا مواجه شد.");
            }
        }

        public async Task<OperationResult<object>> CloseTicketAsync(CloseTicketDto dto)
        {
            var url = $"{apiSetting.ApiBaseUrl}Ticket/{dto.TicketId}";
            var requestBody = new
            {
                Status = "9a5e80a8-cc75-46f1-b158-01d58384d4f7" // وضعیت "بسته شده"
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiSetting.ApiToken);
            httpClient.DefaultRequestHeaders.Accept.Clear(); // پاکسازی قبلی‌ها برای احتیاط
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.PutAsync(url, content);

            if (response.IsSuccessStatusCode)
                return OperationResult<object>.Success(null, "تیکت مورد نظر با موفقیت بسته شد.");
            else
                return OperationResult<object>.Failure("بستن این تیکت با خطا مواجه شد.");
        }

    }
}
