using System.Linq;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments;
using Dfe.Rscd.Web.ApiClient;
using Microsoft.AspNetCore.Http;

namespace Dfe.CspdAlpha.Web.Application.Application.Services
{
    public class AmendmentService : IAmendmentService
    {
        private readonly IClient _apiClient;
        private readonly string _checkingWindowUrl;

        public AmendmentService(IClient apiClient, IHttpContextAccessor httpContextAccessor)
        {
            _apiClient = apiClient;
            var checkingWindow = CheckingWindowHelper.GetCheckingWindow(httpContextAccessor.HttpContext.Request.RouteValues);
            _checkingWindowUrl = CheckingWindowHelper.GetCheckingWindowURL(checkingWindow);
        }

        public AmendmentsListViewModel GetAmendmentsListViewModel(string urn)
        {
            var amendments = _apiClient
                .GetAmendmentsAsync(urn, _checkingWindowUrl)
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
            var result = _apiClient.Create_AmendmentAsync(_checkingWindowUrl, amendment).GetAwaiter().GetResult();

            return result.Result;
        }

        public Amendment GetAmendment(string id)
        {
            var response = _apiClient.GetAmendmentAsync(id, _checkingWindowUrl).GetAwaiter().GetResult();
            var apiAmendment = response.Result;

            return apiAmendment;
        }

        public bool CancelAmendment(string id)
        {
            return _apiClient.CancelAmendmentAsync(id, _checkingWindowUrl).GetAwaiter().GetResult().Result;
        }

        public bool RelateEvidence(string amendmentId, string evidenceFolder)
        {
            return _apiClient
                .RelateEvidenceAsync(evidenceFolder, _checkingWindowUrl,amendmentId) 
                .GetAwaiter()
                .GetResult().Result;
        }
    }
}
