using System.Linq;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments;
using Dfe.Rscd.Web.ApiClient;

namespace Dfe.CspdAlpha.Web.Application.Application.Services
{
    public class AmendmentService : ContextAwareService, IAmendmentService
    {
        private readonly IClient _apiClient;

        public AmendmentService(IClient apiClient)
        {
            _apiClient = apiClient;
        }

        public AmendmentsListViewModel GetAmendmentsListViewModel(string urn)
        {
            var amendments = _apiClient
                .GetAmendmentsAsync(urn, CheckingWindowUrl)
                .GetAwaiter()
                .GetResult();

            return new AmendmentsListViewModel
            {
                Urn = urn,
                AmendmentList = amendments.Result
                    .Select(a => new AmendmentListItem
                    {
                        FirstName = a.Pupil.Forename,
                        LastName = a.Pupil.Surname,
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

        public AmendmentOutcome CreateAmendment(Amendment amendment)
        {
            var result = _apiClient.Create_AmendmentAsync(CheckingWindowUrl, amendment).GetAwaiter().GetResult();

            return result.Result;
        }

        public Amendment GetAmendment(string id)
        {
            var response = _apiClient.GetAmendmentAsync(id, CheckingWindowUrl).GetAwaiter().GetResult();
            var apiAmendment = response.Result;

            return apiAmendment;
        }

        public bool CancelAmendment(string id)
        {
            return _apiClient.CancelAmendmentAsync(id, CheckingWindowUrl).GetAwaiter().GetResult().Result;
        }

        public bool RelateEvidence(string amendmentId, string evidenceFolder)
        {
            return _apiClient
                .RelateEvidenceAsync(evidenceFolder, CheckingWindowUrl,amendmentId) 
                .GetAwaiter()
                .GetResult().Result;
        }
    }
}
