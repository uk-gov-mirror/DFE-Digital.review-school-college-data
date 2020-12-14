using System.Linq;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments;
using Dfe.Rscd.Web.ApiClient;

namespace Dfe.CspdAlpha.Web.Application.Application.Services
{
    public class AmendmentService : IAmendmentService
    {
        private readonly IClient _apiClient;

        public AmendmentService(IClient apiClient)
        {
            _apiClient = apiClient;
        }

        public AmendmentsListViewModel GetAmendmentsListViewModel(string urn, CheckingWindow checkingWindow)
        {
            var checkingWindowUrl = CheckingWindowHelper.GetCheckingWindowURL(checkingWindow);

            var amendments = _apiClient
                .GetAmendmentsAsync(urn, checkingWindowUrl)
                .GetAwaiter()
                .GetResult();

            return new AmendmentsListViewModel
            {
                Urn = urn,
                AmendmentList = amendments.Result
                    .Select(a => new AmendmentListItem
                    {
                        CheckingWindow = a.CheckingWindow,
                        FirstName = a.Pupil.ForeName,
                        LastName = a.Pupil.LastName,
                        PupilId = a.Pupil.Id,
                        DateRequested = a.CreatedDate.DateTime,
                        ReferenceId = a.Reference,
                        Id = a.Id,
                        Status = a.Status,
                        EvidenceStatus = a.EvidenceStatus
                    })
                    .OrderByDescending(a => a.DateRequested)
                    .ToList()
            };
        }

        public string CreateAmendment(Amendment amendment)
        {
            var checkingWindowURL = CheckingWindowHelper.GetCheckingWindowURL(amendment.CheckingWindow);
            var result = _apiClient.Create_AmendmentAsync(checkingWindowURL, amendment).GetAwaiter().GetResult();

            return result.Result;
        }

        public Amendment GetAmendment(CheckingWindow checkingWindow, string id)
        {
            var response = _apiClient.GetAmendmentAsync(id, checkingWindow.ToString()).GetAwaiter().GetResult();
            var apiAmendment = response.Result;

            return apiAmendment;
        }

        public bool CancelAmendment(CheckingWindow checkingWindow, string id)
        {
            return _apiClient.CancelAmendmentAsync(id, checkingWindow.ToString()).GetAwaiter().GetResult().Result;
        }

        public bool RelateEvidence(CheckingWindow checkingWindow, string amendmentId, string evidenceFolder)
        {
            return _apiClient
                .RelateEvidenceAsync(checkingWindow.ToString(), amendmentId, evidenceFolder)
                .GetAwaiter()
                .GetResult().Result;
        }
    }
}
