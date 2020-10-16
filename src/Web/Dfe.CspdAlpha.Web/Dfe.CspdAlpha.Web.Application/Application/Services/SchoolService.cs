using System;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using Dfe.Rscd.Web.ApiClient;
using CheckingWindow = Dfe.CspdAlpha.Web.Application.Models.Common.CheckingWindow;

namespace Dfe.CspdAlpha.Web.Application.Application.Services
{
    public class SchoolService : ISchoolService
    {
        private IEstablishmentService _establishmentService;
        private IClient _apiClient;

        public SchoolService(IEstablishmentService establishmentService, IClient apiClient)
        {
            _apiClient = apiClient;
            _establishmentService = establishmentService;
        }

        public TaskListViewModel GetConfirmationRecord(CheckingWindow checkingWindow, string userId, string urn)
        {
            var schoolDetails = _establishmentService.GetSchoolDetails(checkingWindow, urn);
            var schoolReviewRecord = GetConfirmationRecordFromCRM(checkingWindow, userId, urn);
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
                DataConfirmed = schoolReviewRecord.DataConfirmed
            };
        }

        private ConfirmationRecord GetConfirmationRecordFromCRM(CheckingWindow checkingWindow, string userId, string urn)
        {
            try
            {
                var schoolReviewRecord = _apiClient.GetSchoolReviewRecordAsync(userId, urn, checkingWindow.ToString())
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

        public bool UpdateConfirmation(CheckingWindow checkingWindow, TaskListViewModel taskListViewModel, string userId, string urn)
        {
            var schoolReviewRecord = GetConfirmationRecordFromCRM(checkingWindow, userId, urn);
            if (schoolReviewRecord == null)
            {
                return _apiClient.CreateSchoolReviewRecordAsync(checkingWindow.ToString(), new Rscd.Web.ApiClient.ConfirmationRecord 
                {
                    UserId = userId,
                    EstablishmentId = urn,
                    ReviewCompleted = taskListViewModel.ReviewChecked,
                    DataConfirmed = taskListViewModel.DataConfirmed
                }).GetAwaiter().GetResult().Result;
            }

            schoolReviewRecord.ReviewCompleted = taskListViewModel.ReviewChecked;
            schoolReviewRecord.DataConfirmed = taskListViewModel.DataConfirmed;
            return _apiClient.UpdateSchoolReviewRecordAsync(checkingWindow.ToString(), schoolReviewRecord).GetAwaiter().GetResult().Result;
        }
    }
}
