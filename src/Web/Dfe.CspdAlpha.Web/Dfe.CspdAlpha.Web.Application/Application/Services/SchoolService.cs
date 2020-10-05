using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.School;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Dfe.CspdAlpha.Web.Domain.Entities;
using Dfe.CspdAlpha.Web.Domain.Interfaces;using System.Linq;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Results;
using Dfe.Rscd.Web.ApiClient;
using Establishment = Dfe.Rscd.Web.ApiClient.Establishment;
using Gender = Dfe.Rscd.Web.ApiClient.Gender;
using Ks2Subject = Dfe.CspdAlpha.Web.Application.Models.ViewModels.Results.Ks2Subject;

namespace Dfe.CspdAlpha.Web.Application.Application.Services
{
    public class SchoolService : ISchoolService
    {
        private IConfirmationService _confirmationService;
        private IClient _apiClient;

        public SchoolService(IConfirmationService confirmationService, IClient apiClient)
        {
            _apiClient = apiClient;
            _confirmationService = confirmationService;
        }

        private Establishment GetEstablishmentData(CheckingWindow checkingWindow, string urn)
        {
            var school = _apiClient.EstablishmentsAsync(urn, checkingWindow.ToString().ToLower()).GetAwaiter().GetResult();
            if (string.IsNullOrWhiteSpace(school.Error.ErrorMessage))
            {
                return school.Result;
            }
            return null;
        }

        public PupilListViewModel GetPupilListViewModel(CheckingWindow checkingWindow, SearchQuery searchQuery)
        {
            var checkingWindowURL = CheckingWindowHelper.GetCheckingWindowURL(checkingWindow);
            var schoolDetails = GetSchoolDetails(checkingWindow, searchQuery.URN);
            var pupilDetails = _apiClient.Search2Async(searchQuery.URN, searchQuery.SearchType == QueryType.Name ? searchQuery.Query : string.Empty, searchQuery.SearchType == QueryType.PupilID ? searchQuery.Query : string.Empty, checkingWindowURL).GetAwaiter().GetResult();
            return new PupilListViewModel
            {
                Urn = searchQuery.URN,
                SchoolDetails = schoolDetails,
                Pupils = pupilDetails.Result
                    .Select(p => new PupilListEntry { FirstName = p.ForeName, LastName = p.LastName, PupilId = p.Id, UPN = p.Upn })
                    .OrderBy(p => p.FirstName)
                    .ToList()
            };
        }

        public MatchedPupilViewModel GetPupil(CheckingWindow checkingWindow, string id)
        {
            var checkingWindowURL = CheckingWindowHelper.GetCheckingWindowURL(checkingWindow);
            var pupil = _apiClient.PupilsAsync(id, checkingWindowURL).GetAwaiter().GetResult();
            if (pupil == null)
            {
                return null;
            }

            return GetMatchedPupilViewModel(pupil.Result);
        }


        private MatchedPupilViewModel GetMatchedPupilViewModel(Rscd.Web.ApiClient.Pupil pupil)
        {
            return new MatchedPupilViewModel()
            {
                PupilViewModel = new PupilViewModel
                {
                    URN = pupil.Urn.Value,
                    UPN = pupil.Upn,
                    SchoolID = pupil.LaEstab,
                    FirstName = pupil.ForeName,
                    LastName = pupil.LastName,
                    DateOfBirth = pupil.DateOfBirth.Date,
                    Age = pupil.Age,
                    Gender = pupil.Gender == Gender._1
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

        public MatchedPupilViewModel GetMatchedPupil(CheckingWindow checkingWindow, string upn)
        {
            var checkingWindowURL = CheckingWindowHelper.GetCheckingWindowURL(checkingWindow);
            var pupil = _apiClient.Search2Async(string.Empty, string.Empty, upn, checkingWindowURL).GetAwaiter()
                .GetResult();
            if (pupil == null || pupil.Result == null || pupil.Result.Count == 0 || pupil.Result.Count > 1)
            {
                return null;
            }

            return GetMatchedPupilViewModel(pupil.Result.First());
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

        public TaskListViewModel GetConfirmationRecord(CheckingWindow checkingWindow, string userId, string urn)
        {
            var confirmationRecord = _confirmationService.GetConfirmationRecord(userId, urn);
            return confirmationRecord != null
                ? new TaskListViewModel
                {
                    SchoolDetails = GetSchoolDetails(checkingWindow, urn),
                    ReviewChecked = confirmationRecord.ReviewCompleted, DataConfirmed = confirmationRecord.DataConfirmed
                }
                : new TaskListViewModel
                {
                    SchoolDetails = GetSchoolDetails(checkingWindow, urn)
                };
        }

        private SchoolDetails GetSchoolDetails(CheckingWindow checkingWindow, string urn)
        {
            var establishmentData = GetEstablishmentData(checkingWindow, urn);
            return new SchoolDetails
            {
                SchoolName = establishmentData.Name,
                URN = urn,
                LAEstab = establishmentData.LaEstab,
                SchoolType = establishmentData.SchoolType
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
