using Dfe.CspdAlpha.Web.Application.Application.Extensions;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments;
using System;
using System.Collections.Generic;
using System.Linq;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Amendment = Dfe.CspdAlpha.Web.Application.Models.School.Amendment;
using ApiClient = Dfe.Rscd.Web.ApiClient;

namespace Dfe.CspdAlpha.Web.Application.Application.Services
{
    public class AmendmentService : IAmendmentService
    {
        private ApiClient.IClient _apiClient;

        public AmendmentService(ApiClient.IClient apiClient )
        {
            _apiClient = apiClient;
        }

        public AmendmentsListViewModel GetAmendmentsListViewModel(string urn, CheckingWindow checkingWindow)
        {
            var checkingWindowURL = CheckingWindowHelper.GetCheckingWindowURL(checkingWindow);
            var amendments = _apiClient.GetAmendmentsAsync(urn, checkingWindowURL).GetAwaiter().GetResult();
            return  new AmendmentsListViewModel
            {
                Urn = urn,
                AmendmentList = amendments.Result
                    .Select(a => new Amendment
                    {
                        CheckingWindow = a.CheckingWindow.ToApplicationCheckingWindow(),
                        FirstName = a.Pupil.ForeName,
                        LastName = a.Pupil.LastName,
                        PupilId = a.Pupil.Id,
                        DateRequested = a.CreatedDate.DateTime,
                        ReferenceId = a.Reference,
                        Id = a.Id,
                        Status = a.Status,
                        EvidenceStatus = a.EvidenceStatus.ToApplicationEvidenceOption()
                    })
                    .OrderByDescending(a => a.DateRequested)
                    .ToList()
            };
        }

        public Dictionary<string, string> GetRemoveReasons(string reason = null)
        {
            if (reason == "329")
            {
                return new Dictionary<string, string>
                {
                    { "01", "Excluded" },
                    { "02", "Left on health grounds" },
                    { "03", "Pregnancy" },
                    { "04", "Exceptional circumstances" },
                    { "05", "Imprisoned/Trial" },
                    { "06", "Safeguarding" },
                    { "07", "Not ESFA funded" },
                    { "08", "Age not eligible" },
                    { "09", "Wrong year group" },
                    { "10", "Census error" }
                };
            }

            if (reason == "330")
            {
                return new Dictionary<string, string>
                {
                    { "01", "Part-time learner" },
                    { "02", "Work based learner at FE College" },
                    { "03", "On a one year course of study" },
                    { "04", "Work experience" },
                    { "05", "Below level 2 students" },
                    { "06", "Left for apprenticeship but previously on roll" },
                    { "07", "Withdrawn from exam/no exams taken" },
                    { "08", "Not at end of studies" },
                    { "09", "Left country permanently" },
                    { "10", "Private candidate" },
                    { "12", "Small programme" },
                    { "13", "Died" },
                    { "14", "SEN" },
                    { "15", "Non-attendance" },
                    { "16", "Moved provider" },
                    { "20", "Previously reported" },
                    { "21", "Dual registered" }
                };
            }
            return new Dictionary<string, string>
            {
                { "325", "Not at the end of 16 to 18 study" },
                { "326", "International student" },
                { "327", "Deceased" },
                { "328", "Not on roll" },
                { "329", "Other - with evidence" },
                { "330", "Other - evidence not required" },
            };
        }

        public string CreateAmendment(Dfe.CspdAlpha.Web.Application.Models.Amendments.Amendment amendment)
        {
            var checkingWindowURL = CheckingWindowHelper.GetCheckingWindowURL(amendment.CheckingWindow);
            var amendmentDto = new ApiClient.AmendmentDTO();
            amendmentDto.Amendment = new ApiClient.Amendment
            {
                CheckingWindow = amendment.CheckingWindow.ToApiCheckingWindow(),
                AmendmentType = amendment.AmendmentDetail.AmendmentType.ToApiAmendmentType(),
                Urn = amendment.URN,
                Pupil = new ApiClient.Pupil
                {
                    Id = amendment.PupilDetails.ID,
                    ForeName = amendment.PupilDetails.FirstName,
                    LastName = amendment.PupilDetails.LastName,
                    Gender = amendment.PupilDetails.Gender.ToApiGender(),
                    DateOfBirth = amendment.PupilDetails.DateOfBirth,
                    DateOfAdmission = amendment.PupilDetails.DateOfAdmission,
                    Age = amendment.PupilDetails.Age,
                    Upn = amendment.PupilDetails.UPN,
                    Uln = amendment.PupilDetails.ULN,
                    LaEstab = amendment.PupilDetails.LAEstab,
                    YearGroup = amendment.PupilDetails.YearGroup
                },
                EvidenceStatus = amendment.EvidenceOption.ToApiEvidenceStatus(),
                EvidenceFolderName = amendment.EvidenceFolderName
            };
            // TODO: need to add other amendment types here
            if (amendmentDto.Amendment.AmendmentType == ApiClient.AmendmentType.AddPupil)
            {
                var addPupil = (Dfe.CspdAlpha.Web.Application.Models.Amendments.AmendmentTypes.AddPupil)amendment.AmendmentDetail;
                amendmentDto.AddPupil = new ApiClient.AddPupil
                {
                    Reason = addPupil.AddReason.ToApiAddReason(),
                    PreviousSchoolLAEstab = addPupil.PreviousSchoolLAEstab,
                    PreviousSchoolURN = addPupil.PreviousSchoolURN,
                    PriorAttainmentResults = addPupil.PriorAttainmentResults.Select(r =>
                        new ApiClient.PriorAttainment
                        {
                            Subject = r.Ks2Subject.ToApiKs2Subject(),
                            ExamYear = r.ExamYear,
                            TestMark = r.Mark,
                            ScaledScore = r.ScaledScore
                        }).ToList()
                };
            }
            if (amendmentDto.Amendment.AmendmentType == ApiClient.AmendmentType.RemovePupil)
            {
                var removeAmendment = (Dfe.CspdAlpha.Web.Application.Models.Amendments.AmendmentTypes.RemovePupil) amendment.AmendmentDetail;
                amendmentDto.RemovePupil = new ApiClient.RemovePupil
                {
                    Reason = removeAmendment.Reason,
                    SubReason = removeAmendment.SubReason,
                    Detail = removeAmendment.Detail
                };
            }

            var result = _apiClient.CreateAmendmentAsync(checkingWindowURL, amendmentDto).GetAwaiter().GetResult();

            return result.Result;
        }

        public AmendmentViewModel GetAmendment(CheckingWindow checkingWindow, Guid id)
        {
            var getAmendmentResult = _apiClient.GetAmendmentAsync(id.ToString(), checkingWindow.ToString()).GetAwaiter()
                .GetResult();
            var pupil = getAmendmentResult.Result.Pupil;
            return new AmendmentViewModel
            {
                PupilViewModel = new PupilViewModel
                {
                    UPN = pupil.Upn,
                    FirstName = pupil.ForeName,
                    LastName = pupil.LastName,
                    DateOfBirth = pupil.DateOfBirth.Date,
                    Gender = pupil.Gender.ToApplicationGender(),
                    Age = pupil.Age,
                    DateOfAdmission = pupil.DateOfAdmission.Date,
                    YearGroup = pupil.YearGroup
                }
                // Results = amendment.PriorAttainmentResults.Select(r => new PriorAttainmentResultViewModel() { Subject = GetSubject(r.Subject), ExamYear = r.ExamYear, TestMark = r.TestMark, ScaledScore = r.ScaledScore }).ToList(),
            };
        }

        public bool CancelAmendment(CheckingWindow checkingWindow, string id)
        {
            return _apiClient.CancelAmendmentAsync(id, checkingWindow.ToString()).GetAwaiter().GetResult().Result;
        }

        public bool RelateEvidence(CheckingWindow checkingWindow, string amendmentid, string evidencefolder)
        {
            return _apiClient
                .RelateEvidenceAsync(checkingWindow.ToString(),
                    new ApiClient.RelateEvidenceDTO
                    {
                        AmendmentID = amendmentid,
                        EvidenceFolderName = evidencefolder
                    }).GetAwaiter()
                .GetResult().Result;
        }
    }
}
