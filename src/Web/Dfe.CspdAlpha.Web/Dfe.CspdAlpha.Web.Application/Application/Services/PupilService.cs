using System;
using System.Collections.Generic;
using System.Globalization;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Results;
using ApiClient = Dfe.Rscd.Web.ApiClient;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Dfe.CspdAlpha.Web.Application.Application.Services
{
    public class PupilService : ContextAwareService, IPupilService
    {
        private readonly ApiClient.IClient _apiClient;

        public PupilService(ApiClient.IClient apiClient)
        {
            _apiClient = apiClient;
        }

        public List<PupilViewModel> GetPupilDetailsList(SearchQuery searchQuery)
        {
            var pupilDetails = _apiClient
                .SearchPupilsAsync(searchQuery.URN, searchQuery.SearchType == QueryType.Name ? searchQuery.Query : string.Empty,
                    searchQuery.SearchType == QueryType.PupilID ? searchQuery.Query : string.Empty,
                    CheckingWindowUrl)
                .GetAwaiter()
                .GetResult();

            return pupilDetails.Result
                .Select(p => new PupilViewModel
                {
                    FirstName = p.ForeName,
                    LastName = p.Surname,
                    ID = p.Id,
                    UPN = p.Upn,
                    ULN = p.Uln,
                    DateOfBirth = GetDateTime(p.DateOfBirth),
                    Gender = new ApiClient.Gender{ Code = p.Gender}
                })
                .OrderBy(p => p.FirstName)
                .ToList();
        }

        private DateTime GetDateTime(string dateString)
        {
            return DateTime.TryParseExact(dateString, "yyyyMMdd", new CultureInfo("en-GB"), DateTimeStyles.None,
                out var date)
                ? date
                : DateTime.MinValue;
        }

        public MatchedPupilViewModel GetPupil(string id)
        {
            var pupil = _apiClient.GetPupilByIdAsync(id, CheckingWindowUrl).GetAwaiter().GetResult();
            if (pupil == null)
            {
                return null;
            }

            return GetMatchedPupilViewModel(pupil.Result);
        }

        private MatchedPupilViewModel GetMatchedPupilViewModel(ApiClient.Pupil pupil)
        {
            return new MatchedPupilViewModel()
            {
                PupilViewModel = new PupilViewModel(pupil),
                Results = pupil.Results
                    .Select(r => new PriorAttainmentResultViewModel
                    {
                        Subject = GetSubject(r.SubjectCode),
                        ExamYear = ValidateValue(r.ExamYear),
                        TestMark = ValidateValue(r.TestMark),
                        ScaledScore = ValidateValue(r.ScaledScore)
                    }).Where(r => r.Subject != Ks2Subject.Unknown).ToList()
            };
        }

        public MatchedPupilViewModel GetMatchedPupil(string upn)
        {
            var pupilResults = _apiClient.SearchPupilsAsync(string.Empty, string.Empty, upn, CheckingWindowUrl).GetAwaiter()
                .GetResult();

            if (pupilResults == null || pupilResults.Result == null || pupilResults.Result.Count == 0 || pupilResults.Result.Count > 1)
            {
                return null;
            }

            var pupil = _apiClient.GetPupilByIdAsync(pupilResults.Result.First().Id, CheckingWindowUrl).GetAwaiter().GetResult();

            return GetMatchedPupilViewModel(pupil.Result);
        }

        public List<ApiClient.AmendmentReason> GetAmendmentReasons(string pinclId)
        {
            var results = _apiClient.GetAmendmentReasonsAsync(pinclId, CheckingWindowUrl).GetAwaiter()
                .GetResult();

            return results.Result.ToList();
        }

        private string ValidateValue(string value)
        {
            return string.IsNullOrWhiteSpace(value) || value.Trim().ToLower() == "null" ? string.Empty : value.Trim();
        }

        private string GetSubject(string subjectCode)
        {
            switch (subjectCode)
            {
                case "9982":
                    return Ks2Subject.Maths;
                case "9984":
                    return Ks2Subject.Reading;
                case "9985":
                    return Ks2Subject.Writing;
                default:
                    return Ks2Subject.Unknown;
            }
        }
    }
}
