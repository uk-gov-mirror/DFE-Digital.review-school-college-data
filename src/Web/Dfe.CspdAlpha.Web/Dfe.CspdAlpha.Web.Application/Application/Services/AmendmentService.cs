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

            // TODO: CHECK AMENDMENT DETAILS
            //if (amendment.AmendmentType == ApiClient.AmendmentType.AddPupil)
            //{
            //    var addPupil = amendment.AmendmentDetail;
            //    amendment.AmendmentDetail = new ApiClient.IAmendmentDetail()
            //    {
            //        Reason = addPupil.AddReason.ToApiAddReason(),
            //        PreviousSchoolLAEstab = addPupil.PreviousSchoolLAEstab,
            //        PreviousSchoolURN = addPupil.PreviousSchoolURN,
            //        PriorAttainmentResults = addPupil.PriorAttainmentResults.Select(r =>
            //            new ApiClient.PriorAttainment
            //            {
            //                Subject = r.Ks2Subject.ToApiKs2Subject(),
            //                ExamYear = r.ExamYear,
            //                TestMark = r.Mark,
            //                ScaledScore = r.ScaledScore
            //            }).ToList()
            //    };
            //}
            //if (amendmentDto.Amendment.AmendmentType == ApiClient.AmendmentType.RemovePupil)
            //{
            //    var removeAmendment = (RemovePupil) amendment.AmendmentDetail;
            //    amendmentDto.RemovePupil = new ApiClient.RemovePupil
            //    {
            //        ReasonCode = removeAmendment.ReasonCode,
            //        SubReason = removeAmendment.SubReason,
            //        Detail = removeAmendment.Detail
            //    };
            //}

            var result = _apiClient.Create_AmendmentAsync(checkingWindowURL, amendment).GetAwaiter().GetResult();

            return result.Result;
        }

        public Amendment GetAmendment(CheckingWindow checkingWindow, string id)
        {
            var response = _apiClient.GetAmendmentAsync(id, checkingWindow.ToString()).GetAwaiter().GetResult();
            var apiAmendment = response.Result;

            //TODO: Check API Amendment Details coming through

            //var amendment = new ApiClient.Amendment
            //{
            //    CheckingWindow = apiAmendment.CheckingWindow.ToApplicationCheckingWindow(),
            //    URN = apiAmendment.Urn,
            //    PupilDetails = new PupilDetails
            //    {
            //        ID = apiAmendment.Pupil.Id,
            //        KeyStage = apiAmendment.CheckingWindow.ToKeyStage(),
            //        URN = apiAmendment.Pupil.Urn,
            //        LAEstab = apiAmendment.Pupil.LaEstab,
            //        UPN = apiAmendment.Pupil.Upn,
            //        ULN = apiAmendment.Pupil.Uln,
            //        FirstName = apiAmendment.Pupil.ForeName,
            //        LastName = apiAmendment.Pupil.LastName,
            //        DateOfBirth = apiAmendment.Pupil.DateOfBirth.Date,
            //        Age = apiAmendment.Pupil.Age,
            //        Gender = apiAmendment.Pupil.Gender.ToApplicationGender(),
            //        DateOfAdmission = apiAmendment.Pupil.DateOfAdmission.Date,
            //        YearGroup = apiAmendment.Pupil.YearGroup,
            //        AllocationYears = apiAmendment.Pupil.Allocations
            //            .Select(x => x.Year)
            //            .OrderByDescending(x => x)
            //            .ToArray()
            //    },
            //    EvidenceOption = apiAmendment.EvidenceStatus.ToApplicationEvidenceOption()
            //};
            //if(apiAmendment.AmendmentType == ApiClient.AmendmentType.AddPupil)
            //{
            //    var apiAddPupil = response.Result.AddPupil;
            //    amendment.AmendmentDetail = new AddPupil
            //    {
            //        AddReason = apiAddPupil.Reason == ApiClient.AddReason.New ? AddReason.New : AddReason.Existing,
            //        PreviousSchoolURN = apiAddPupil.PreviousSchoolURN,
            //        PreviousSchoolLAEstab = apiAddPupil.PreviousSchoolLAEstab,
            //        PriorAttainmentResults = apiAddPupil.PriorAttainmentResults.Select(r => new PriorAttainmentResult{ Ks2Subject = r.Subject.ToApplicationKs2Subject(), ExamYear = r.ExamYear, Mark = r.TestMark, ScaledScore = r.ScaledScore} ).ToList()
            //    };
            //}
            //else
            //{
            //    var apiRemovePupil = response.Result.RemovePupil;
            //    amendment.AmendmentDetail = new RemovePupil
            //    {
            //        ReasonCode = apiRemovePupil.ReasonCode,
            //        SubReason = apiRemovePupil.SubReason,
            //        Detail = apiRemovePupil.Detail
            //    };
            //}

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
