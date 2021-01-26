using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using Dfe.Rscd.Web.ApiClient;

namespace Dfe.CspdAlpha.Web.Application.Application.Services
{
    public class SchoolService : ContextAwareService, ISchoolService
    {
        private readonly IClient _apiClient;
        private readonly IEstablishmentService _establishmentService;

        public SchoolService(IEstablishmentService establishmentService, IClient apiClient)
        {
            _apiClient = apiClient;
            _establishmentService = establishmentService;
        }

        public TaskListViewModel GetConfirmationRecord(string userId, string urn)
        {
            var schoolDetails = _establishmentService.GetSchoolDetails(urn);
            var schoolReviewRecord = GetConfirmationRecordFromCRM(userId, urn);
            if (schoolReviewRecord == null)
                return new TaskListViewModel
                {
                    SchoolDetails = schoolDetails
                };

            return new TaskListViewModel
            {
                SchoolDetails = schoolDetails,
                ReviewChecked = schoolReviewRecord.ReviewCompleted,
                DataConfirmed = schoolReviewRecord.DataConfirmed,
                ConfirmationDate = schoolReviewRecord.ConfirmationDate.Date
            };
        }

        public bool UpdateConfirmation(TaskListViewModel taskListViewModel, string userId, string urn)
        {
            var schoolReviewRecord = GetConfirmationRecordFromCRM(userId, urn);
            if (schoolReviewRecord == null)
                return _apiClient.CreateSchoolReviewRecordAsync(CheckingWindowUrl, new ConfirmationRecord
                {
                    UserId = userId,
                    EstablishmentId = urn,
                    ReviewCompleted = taskListViewModel.ReviewChecked,
                    DataConfirmed = taskListViewModel.DataConfirmed
                }).GetAwaiter().GetResult().Result;

            schoolReviewRecord.ReviewCompleted = taskListViewModel.ReviewChecked;
            schoolReviewRecord.DataConfirmed = taskListViewModel.DataConfirmed;
            return _apiClient.UpdateSchoolReviewRecordAsync(CheckingWindowUrl, schoolReviewRecord).GetAwaiter()
                .GetResult().Result;
        }

        private ConfirmationRecord GetConfirmationRecordFromCRM(string userId, string urn)
        {
            try
            {
                var schoolReviewRecord = _apiClient.GetSchoolReviewRecordAsync(userId, urn, CheckingWindowUrl)
                    .GetAwaiter().GetResult();
                return schoolReviewRecord.Result;
            }
            catch (ApiException e)
            {
                if (e.StatusCode == 404) return null;

                throw;
            }
        }
    }
}
