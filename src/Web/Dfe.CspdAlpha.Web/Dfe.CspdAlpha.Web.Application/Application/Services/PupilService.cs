using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Results;
using ApiClient = Dfe.Rscd.Web.ApiClient;
using System.Linq;

namespace Dfe.CspdAlpha.Web.Application.Application.Services
{
    public class PupilService : IPupilService
    {
        private ApiClient.IClient _apiClient;

        public PupilService(ApiClient.IClient apiClient)
        {
            _apiClient = apiClient;
        }

        public List<PupilDetails> GetPupilDetailsList(ApiClient.CheckingWindow checkingWindow, SearchQuery searchQuery)
        {
            var checkingWindowURL = CheckingWindowHelper.GetCheckingWindowURL(checkingWindow);
            var pupilDetails = _apiClient.SearchPupilsAsync(searchQuery.URN, searchQuery.SearchType == QueryType.Name ? searchQuery.Query : string.Empty, searchQuery.SearchType == QueryType.PupilID ? searchQuery.Query : string.Empty, checkingWindowURL).GetAwaiter().GetResult();
            return pupilDetails.Result
                .Select(p => new PupilDetails {ForeName = p.ForeName, LastName = p.Surname, Id = p.Id, Upn = p.Upn, Uln = p.Uln})
                .OrderBy(p => p.ForeName)
                .ToList();
        }

        public MatchedPupilViewModel GetPupil(ApiClient.CheckingWindow checkingWindow, string id)
        {
            var checkingWindowUrl = CheckingWindowHelper.GetCheckingWindowURL(checkingWindow);
            var pupil = _apiClient.GetPupilByIdAsync(id, checkingWindowUrl).GetAwaiter().GetResult();
            if (pupil == null)
            {
                return null;
            }

            return GetMatchedPupilViewModel(pupil.Result, checkingWindow);
        }

        private MatchedPupilViewModel GetMatchedPupilViewModel(ApiClient.Pupil pupil, ApiClient.CheckingWindow checkingWindow)
        {
            return new MatchedPupilViewModel()
            {
                PupilViewModel = new PupilViewModel
                {
                    ID = pupil.Id,
                    Keystage = GetKeyStage(checkingWindow),
                    URN = pupil.Urn,
                    SchoolID = pupil.LaEstab,
                    UPN = pupil.Upn,
                    ULN = pupil.Uln,
                    FirstName = pupil.ForeName,
                    LastName = pupil.LastName,
                    DateOfBirth = pupil.DateOfBirth.Date,
                    Age = pupil.Age,
                    Gender = pupil.Gender,
                    DateOfAdmission = pupil.DateOfAdmission.Date,
                    YearGroup = pupil.YearGroup,
                    AllocationYears = pupil.Allocations
                        .Select(x=>x.Year)
                        .OrderByDescending(x=>x)
                        .ToArray()
                },
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

        private Keystage GetKeyStage(ApiClient.CheckingWindow checkingWindow)
        {
            var checkingWindowString = checkingWindow.ToString();
            if (checkingWindowString.ToLower().StartsWith("ks2"))
            {
                return Keystage.KS2;
            }
            if (checkingWindowString.ToLower().StartsWith("ks4"))
            {
                return Keystage.KS4;
            }
            if (checkingWindowString.ToLower().StartsWith("ks5"))
            {
                return Keystage.KS5;
            }

            return Keystage.Unknown;
        }

        public MatchedPupilViewModel GetMatchedPupil(ApiClient.CheckingWindow checkingWindow, string upn)
        {
            var checkingWindowURL = CheckingWindowHelper.GetCheckingWindowURL(checkingWindow);
            var pupilResults = _apiClient.SearchPupilsAsync(string.Empty, string.Empty, upn, checkingWindowURL).GetAwaiter()
                .GetResult();

            if (pupilResults == null || pupilResults.Result == null || pupilResults.Result.Count == 0 || pupilResults.Result.Count > 1)
            {
                return null;
            }

            var pupil = _apiClient.GetPupilByIdAsync(pupilResults.Result.First().Id, checkingWindowURL).GetAwaiter().GetResult();

            return GetMatchedPupilViewModel(pupil.Result, checkingWindow);
        }

        private string ValidateValue(string value)
        {
            return string.IsNullOrWhiteSpace(value) || value.Trim().ToLower() == "null" ? string.Empty : value.Trim();
        }

        private Ks2Subject GetSubject(string subjectCode)
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
