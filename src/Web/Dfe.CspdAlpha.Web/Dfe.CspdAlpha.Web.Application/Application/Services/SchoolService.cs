using System;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using Dfe.Rscd.Web.ApiClient;
using Microsoft.AspNetCore.Http;

namespace Dfe.CspdAlpha.Web.Application.Application.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly IEstablishmentService _establishmentService;
        private readonly IClient _apiClient;
        private readonly string _checkingWindowUrl;

        public SchoolService(IEstablishmentService establishmentService, IClient apiClient, IHttpContextAccessor httpContextAccessor)
        {
            _apiClient = apiClient;
            _establishmentService = establishmentService;
            var checkingWindow = CheckingWindowHelper.GetCheckingWindow(httpContextAccessor.HttpContext.Request.RouteValues);
            _checkingWindowUrl = CheckingWindowHelper.GetCheckingWindowURL(checkingWindow);
        }

        public TaskListViewModel GetConfirmationRecord(string userId, string urn)
        {
            var schoolDetails = _establishmentService.GetSchoolDetails(urn);
            var schoolReviewRecord = GetConfirmationRecordFromCRM(userId, urn);
            if (schoolReviewRecord == null)
            {
                return new TaskListViewModel
                {
                    SchoolDetails = schoolDetails
                };
            }

            return new TaskListViewModel
            {
                SchoolDetails = schoolDetails,
                ReviewChecked = schoolReviewRecord.ReviewCompleted,
                DataConfirmed = schoolReviewRecord.DataConfirmed,
                ConfirmationDate = schoolReviewRecord.ConfirmationDate.Date
            };
        }

        private ConfirmationRecord GetConfirmationRecordFromCRM(string userId, string urn)
        {
            try
            {
                var schoolReviewRecord = _apiClient.GetSchoolReviewRecordAsync(userId, urn, _checkingWindowUrl)
                    .GetAwaiter().GetResult();
                return schoolReviewRecord.Result;
            }
            catch (ApiException e)
            {
                if (e.StatusCode == 404)
                {
                    return null;
                }

                throw;
            }
        }

        public bool UpdateConfirmation(TaskListViewModel taskListViewModel, string userId, string urn)
        {
            var schoolReviewRecord = GetConfirmationRecordFromCRM(userId, urn);
            if (schoolReviewRecord == null)
            {
                return _apiClient.CreateSchoolReviewRecordAsync(_checkingWindowUrl, new Rscd.Web.ApiClient.ConfirmationRecord 
                {
                    UserId = userId,
                    EstablishmentId = urn,
                    ReviewCompleted = taskListViewModel.ReviewChecked,
                    DataConfirmed = taskListViewModel.DataConfirmed
                }).GetAwaiter().GetResult().Result;
            }

            schoolReviewRecord.ReviewCompleted = taskListViewModel.ReviewChecked;
            schoolReviewRecord.DataConfirmed = taskListViewModel.DataConfirmed;
            return _apiClient.UpdateSchoolReviewRecordAsync(_checkingWindowUrl, schoolReviewRecord).GetAwaiter().GetResult().Result;
        }
    }
}
