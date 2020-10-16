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

        public List<PupilDetails> GetPupilDetailsList(CheckingWindow checkingWindow, SearchQuery searchQuery)
        {
            var checkingWindowURL = CheckingWindowHelper.GetCheckingWindowURL(checkingWindow);
            var pupilDetails = _apiClient.SearchPupilsAsync(searchQuery.URN, searchQuery.SearchType == QueryType.Name ? searchQuery.Query : string.Empty, searchQuery.SearchType == QueryType.PupilID ? searchQuery.Query : string.Empty, checkingWindowURL).GetAwaiter().GetResult();
            return pupilDetails.Result
                .Select(p => new PupilDetails {FirstName = p.ForeName, LastName = p.LastName, ID = p.Id, UPN = p.Upn})
                .OrderBy(p => p.FirstName)
                .ToList();
        }

        public MatchedPupilViewModel GetPupil(CheckingWindow checkingWindow, string id)
        {
            var checkingWindowURL = CheckingWindowHelper.GetCheckingWindowURL(checkingWindow);
            var pupil = _apiClient.GetPupilByIdAsync(id, checkingWindowURL).GetAwaiter().GetResult();
            if (pupil == null)
            {
                return null;
            }

            return GetMatchedPupilViewModel(pupil.Result, checkingWindow);
        }

        private MatchedPupilViewModel GetMatchedPupilViewModel(ApiClient.Pupil pupil, CheckingWindow checkingWindow)
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
                    Gender = pupil.Gender == Rscd.Web.ApiClient.Gender.Male
                        ? Models.Common.Gender.Male
                        : Models.Common.Gender.Female,
                    DateOfAdmission = pupil.DateOfAdmission.Date,
                    YearGroup = pupil.YearGroup
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

        private Keystage GetKeyStage(CheckingWindow checkingWindow)
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

        public MatchedPupilViewModel GetMatchedPupil(CheckingWindow checkingWindow, string upn)
        {
            var checkingWindowURL = CheckingWindowHelper.GetCheckingWindowURL(checkingWindow);
            var pupil = _apiClient.SearchPupilsAsync(string.Empty, string.Empty, upn, checkingWindowURL).GetAwaiter()
                .GetResult();
            if (pupil == null || pupil.Result == null || pupil.Result.Count == 0 || pupil.Result.Count > 1)
            {
                return null;
            }

            return GetMatchedPupilViewModel(pupil.Result.First(), checkingWindow);
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
