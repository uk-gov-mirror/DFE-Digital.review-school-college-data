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
            var schoolReviewRecord = _apiClient.GetSchoolReviewRecordAsync(userId, urn, checkingWindow.ToString()).GetAwaiter().GetResult();
            var schoolDetails = _establishmentService.GetSchoolDetails(checkingWindow, urn);
            return schoolReviewRecord.Result != null
                ? new TaskListViewModel
                {
                    SchoolDetails = schoolDetails,
                    ReviewChecked = schoolReviewRecord.Result.ReviewCompleted, DataConfirmed = schoolReviewRecord.Result.DataConfirmed
                }
                : new TaskListViewModel
                {
                    SchoolDetails = schoolDetails
                };
        }

        public bool UpdateConfirmation(CheckingWindow checkingWindow, TaskListViewModel taskListViewModel, string userId, string urn)
        {
            var schoolReviewRecord = _apiClient.GetSchoolReviewRecordAsync(userId, urn, checkingWindow.ToString()).GetAwaiter().GetResult();
            if (schoolReviewRecord.Result == null)
            {
                return _apiClient.CreateSchoolReviewRecordAsync(checkingWindow.ToString(), new Rscd.Web.ApiClient.ConfirmationRecord 
                {
                    UserId = userId,
                    EstablishmentId = urn,
                    ReviewCompleted = taskListViewModel.ReviewChecked,
                    DataConfirmed = taskListViewModel.DataConfirmed
                }).GetAwaiter().GetResult().Result;
            }

            schoolReviewRecord.Result.ReviewCompleted = taskListViewModel.ReviewChecked;
            schoolReviewRecord.Result.DataConfirmed = taskListViewModel.DataConfirmed;
            return _apiClient.UpdateSchoolReviewRecordAsync(checkingWindow.ToString(), schoolReviewRecord.Result).GetAwaiter().GetResult().Result;
        }
    }
}
