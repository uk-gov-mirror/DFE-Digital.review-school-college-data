using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using Dfe.CspdAlpha.Web.Domain.Entities;
using Dfe.CspdAlpha.Web.Domain.Interfaces;

namespace Dfe.CspdAlpha.Web.Application.Application.Services
{
    public class SchoolService : ISchoolService
    {
        private IConfirmationService _confirmationService;
        private IEstablishmentService _establishmentService;

        public SchoolService(IConfirmationService confirmationService, IEstablishmentService establishmentService)
        {
            _establishmentService = establishmentService;
            _confirmationService = confirmationService;
        }

        public TaskListViewModel GetConfirmationRecord(CheckingWindow checkingWindow, string userId, string urn)
        {
            var confirmationRecord = _confirmationService.GetConfirmationRecord(userId, urn);
            var schoolDetails = _establishmentService.GetSchoolDetails(checkingWindow, urn);
            return confirmationRecord != null
                ? new TaskListViewModel
                {
                    SchoolDetails = schoolDetails,
                    ReviewChecked = confirmationRecord.ReviewCompleted, DataConfirmed = confirmationRecord.DataConfirmed
                }
                : new TaskListViewModel
                {
                    SchoolDetails = schoolDetails
                };
        }

        public bool UpdateConfirmation(TaskListViewModel taskListViewModel, string userId, string urn)
        {
            var confirmationRecord = _confirmationService.GetConfirmationRecord(userId, urn);
            if (confirmationRecord == null)
            {
                return _confirmationService.CreateConfirmationRecord(new ConfirmationRecord
                {
                    UserId = userId,
                    EstablishmentId = urn,
                    ReviewCompleted = taskListViewModel.ReviewChecked,
                    DataConfirmed = taskListViewModel.DataConfirmed
                });
            }

            confirmationRecord.ReviewCompleted = taskListViewModel.ReviewChecked;
            confirmationRecord.DataConfirmed = taskListViewModel.DataConfirmed;
            return _confirmationService.UpdateConfirmationRecord(confirmationRecord);
        }
    }
}
