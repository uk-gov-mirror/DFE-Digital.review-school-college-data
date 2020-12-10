using Dfe.CspdAlpha.Web.Application.Application.Extensions;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.Amendments;
using Dfe.CspdAlpha.Web.Application.Models.Amendments.AmendmentTypes;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments;
using System.Collections.Generic;
using System.Linq;
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
                    .Select(a => new AmendmentListItem
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

        public string CreateAmendment(Amendment amendment)
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
                var removeAmendment = (RemovePupil) amendment.AmendmentDetail;
                amendmentDto.RemovePupil = new ApiClient.RemovePupil
                {
                    ReasonCode = removeAmendment.ReasonCode,
                    SubReason = removeAmendment.SubReason,
                    Detail = removeAmendment.Detail,
                    AllocationYears = removeAmendment.AmmendmentYears
                };
            }

            var result = _apiClient.CreateAmendmentAsync(checkingWindowURL, amendmentDto).GetAwaiter().GetResult();

            return result.Result;
        }

        public Amendment GetAmendment(CheckingWindow checkingWindow, string id)
        {
            var response = _apiClient.GetAmendmentAsync(id, checkingWindow.ToString()).GetAwaiter().GetResult();
            var apiAmendment = response.Result.Amendment;
            var amendment = new Amendment
            {
                CheckingWindow = apiAmendment.CheckingWindow.ToApplicationCheckingWindow(),
                URN = apiAmendment.Urn,
                PupilDetails = new PupilDetails
                {
                    ID = apiAmendment.Pupil.Id,
                    Keystage = apiAmendment.CheckingWindow.ToKeyStage(),
                    URN = apiAmendment.Pupil.Urn,
                    LAEstab = apiAmendment.Pupil.LaEstab,
                    UPN = apiAmendment.Pupil.Upn,
                    ULN = apiAmendment.Pupil.Uln,
                    FirstName = apiAmendment.Pupil.ForeName,
                    LastName = apiAmendment.Pupil.LastName,
                    DateOfBirth = apiAmendment.Pupil.DateOfBirth.Date,
                    Age = apiAmendment.Pupil.Age,
                    Gender = apiAmendment.Pupil.Gender.ToApplicationGender(),
                    DateOfAdmission = apiAmendment.Pupil.DateOfAdmission.Date,
                    YearGroup = apiAmendment.Pupil.YearGroup
                },
                EvidenceOption = apiAmendment.EvidenceStatus.ToApplicationEvidenceOption()
            };
            if(apiAmendment.AmendmentType == ApiClient.AmendmentType.AddPupil)
            {
                var apiAddPupil = response.Result.AddPupil;
                amendment.AmendmentDetail = new AddPupil
                {
                    AddReason = apiAddPupil.Reason == ApiClient.AddReason.New ? AddReason.New : AddReason.Existing,
                    PreviousSchoolURN = apiAddPupil.PreviousSchoolURN,
                    PreviousSchoolLAEstab = apiAddPupil.PreviousSchoolLAEstab,
                    PriorAttainmentResults = apiAddPupil.PriorAttainmentResults.Select(r => new PriorAttainmentResult{ Ks2Subject = r.Subject.ToApplicationKs2Subject(), ExamYear = r.ExamYear, Mark = r.TestMark, ScaledScore = r.ScaledScore} ).ToList()
                };
            }
            else
            {
                var apiRemovePupil = response.Result.RemovePupil;
                amendment.AmendmentDetail = new RemovePupil
                {
                    ReasonCode = apiRemovePupil.ReasonCode,
                    SubReason = apiRemovePupil.SubReason,
                    Detail = apiRemovePupil.Detail
                };
            }

            return amendment;
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
