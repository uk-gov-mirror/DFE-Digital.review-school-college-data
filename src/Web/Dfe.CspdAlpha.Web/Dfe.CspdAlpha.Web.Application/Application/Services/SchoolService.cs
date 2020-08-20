using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.School;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Dfe.CspdAlpha.Web.Domain.Core;
using Dfe.CspdAlpha.Web.Domain.Entities;
using Dfe.CspdAlpha.Web.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Results;
using DomainEnums = Dfe.CspdAlpha.Web.Domain.Core.Enums;

namespace Dfe.CspdAlpha.Web.Application.Application.Services
{
    public class SchoolService : ISchoolService
    {
        private IEstablishmentService _establishmentService;
        private readonly Dictionary<string, string> HEADLINE_SCORES = new Dictionary<string, string>
        {
            {"P8MEA", "Progress 8 measure after adjustment for extreme scores" },
            {"ATT8SCR", "Average Attainment 8 score per pupil" },
            {"PTEBACC_95", "Percentage of pupils achieving the English Baccalaureate with 9-5 passes" }
        };
        private readonly Dictionary<string, string> ADDITIONAL_SCORES = new Dictionary<string, string>
        {
            {"EBACCAPS", "Average EBacc APS score per pupil" },
            {"PTEBACC", "Percentage of pupils achieving the English Baccalaureate with 9-4 passes" }
        };
        private readonly Dictionary<string, string> COHORT_SCORES = new Dictionary<string, string>
        {
            {"TPUP", "Number of pupils at the end of key stage 4" },
            {"P8PUP", "Number of pupils included in Progress 8 measure" },
            {"SENSE4", "Number of pupils at the end of key stage 4 with special educational needs (SEN) with a statement or Education, health and care (EHC) plan" }
        };
        private IPupilService _pupilService;
        private IConfirmationService _confirmationService;

        public SchoolService(IEstablishmentService establishmentService, IPupilService pupilService, IConfirmationService confirmationService)
        {
            _confirmationService = confirmationService;
            _pupilService = pupilService;
            _establishmentService = establishmentService;
        }

        public string GetSchoolName(string laestab)
        {
            var school = _establishmentService.GetByLAId(laestab);
            return school != null ? school.Name : string.Empty;
        }

        public SchoolViewModel GetSchoolViewModel(string urn)
        {
            var urnValue = new URN(urn);
            var establishmentData = _establishmentService.GetByURN(urnValue);
            return new SchoolViewModel
            {
                SchoolDetails = new SchoolDetails
                {
                    SchoolName = establishmentData.Name,
                    URN = urn,
                    LAEstab = establishmentData.LaEstab,
                    SchoolType = establishmentData.SchoolType
                },
                HeadlineMeasures = establishmentData.PerformanceMeasures.Where(p => HEADLINE_SCORES.Any(h => h.Key == p.Name)).Select(m => new Measure{Name = HEADLINE_SCORES[m.Name], Data = m.Value}).OrderBy(s => s.Name).ToList(),
                AdditionalMeasures = establishmentData.PerformanceMeasures.Where(p => ADDITIONAL_SCORES.Any(h => h.Key == p.Name)).Select(m => new Measure{Name = ADDITIONAL_SCORES[m.Name], Data = m.Value}).OrderBy(s => s.Name).ToList(),
                CohortMeasures = establishmentData.PerformanceMeasures.Where(p => COHORT_SCORES.Any(h => h.Key == p.Name)).Select(m => new Measure{Name = COHORT_SCORES[m.Name], Data = m.Value}).OrderBy(s => s.Name).ToList()
            };
        }

        public PupilListViewModel GetPupilListViewModel(string urn)
        {
            var urnValue = new URN(urn);
            return new PupilListViewModel
            {
                Urn = urn,
                Pupils = _pupilService
                    .GetByUrn(urnValue)
                    .Select(p => new PupilListEntry { FirstName = p.ForeName, LastName = p.LastName, PupilId = p.Id.Value})
                    .OrderBy(p => p.FirstName)
                    .ToList()
            };
        }

        public MatchedPupilViewModel GetMatchedPupil(string upn)
        {
            var pupil = _pupilService.GetById(new PupilId(upn));
            if (pupil == null)
            {
                return null;
            }

            return new MatchedPupilViewModel()
            {
                PupilViewModel = new PupilViewModel
                {
                    UPN = upn,
                    SchoolID = pupil.LaEstab,
                    FirstName = pupil.ForeName,
                    LastName = pupil.LastName,
                    DateOfBirth = pupil.DateOfBirth,
                    Gender = pupil.Gender == DomainEnums.Gender.Male
                        ? Models.Common.Gender.Male
                        : Models.Common.Gender.Female,
                    DateOfAdmission = pupil.DateOfAdmission,
                    YearGroup = pupil.YearGroup
                },
                Results = pupil.Results
                    .Select(r => new PriorAttainmentResultViewModel
                    {
                        Subject = GetSubject(r.SubjectCode), ExamYear = r.ExamYear, TestMark = r.TestMark,
                        ScaledScore = r.ScaledScore
                    }).Where(r => r.Subject != Ks2Subject.Unknown).ToList()
            };
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

        public TaskListViewModel GetConfirmationRecord(string userId, string urn)
        {
            var confirmationRecord = _confirmationService.GetConfirmationRecord(userId, urn);
            return confirmationRecord != null
                ? new TaskListViewModel
                {
                    SchoolDetails = GetSchoolDetails(urn),
                    ReviewChecked = confirmationRecord.ReviewCompleted, DataConfirmed = confirmationRecord.DataConfirmed
                }
                : new TaskListViewModel
                {
                    SchoolDetails = GetSchoolDetails(urn)
                };
        }

        private SchoolDetails GetSchoolDetails(string urn)
        {
            var urnValue = new URN(urn);
            var establishmentData = _establishmentService.GetByURN(urnValue);
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
