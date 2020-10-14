using System;
using System.Collections.Generic;
using System.Linq;
using Dfe.CspdAlpha.Web.Application.Application.Extensions;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Results;
using Dfe.CspdAlpha.Web.Infrastructure.Interfaces;
using ApiClient = Dfe.Rscd.Web.ApiClient;
using Microsoft.AspNetCore.Http;
using AddPupilAmendment = Dfe.CspdAlpha.Web.Domain.Entities.AddPupilAmendment;
using AddReason = Dfe.CspdAlpha.Web.Application.Models.Common.AddReason;
using Amendment = Dfe.CspdAlpha.Web.Application.Models.School.Amendment;
using Gender = Dfe.CspdAlpha.Web.Application.Models.Common.Gender;
using DomainInterfaces = Dfe.CspdAlpha.Web.Domain.Interfaces;
using EvidenceStatus = Dfe.CspdAlpha.Web.Domain.Core.Enums.EvidenceStatus;
using Ks2Subject = Dfe.CspdAlpha.Web.Application.Models.ViewModels.Results.Ks2Subject;
using PriorAttainment = Dfe.CspdAlpha.Web.Domain.Entities.PriorAttainment;
using Pupil = Dfe.CspdAlpha.Web.Domain.Entities.Pupil;
using URN = Dfe.CspdAlpha.Web.Domain.Core.URN;

namespace Dfe.CspdAlpha.Web.Application.Application.Services
{
    public class AmendmentService : IAmendmentService
    {
        private DomainInterfaces.IAmendmentService _amendmentService;
        private IFileUploadService _fileUploadService;
        private ApiClient.IClient _apiClient;

        public AmendmentService(DomainInterfaces.IAmendmentService amendmentService, IFileUploadService fileUploadService, ApiClient.IClient apiClient )
        {
            _apiClient = apiClient;
            _fileUploadService = fileUploadService;
            _amendmentService = amendmentService;
        }

        public AmendmentsListViewModel GetAmendmentsListViewModel(string urn, CheckingWindow checkingWindow)
        {
            var checkingWindowURL = CheckingWindowHelper.GetCheckingWindowURL(checkingWindow);
            if (checkingWindow == CheckingWindow.KS5)
            {
                var ks5Amendments = _apiClient.GetAmendmentsAsync(urn, checkingWindowURL).GetAwaiter().GetResult();
                return new AmendmentsListViewModel
                {
                    Urn = urn,
                    AmendmentList = ks5Amendments.Result
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
                            EvidenceStatus = GetEvidenceStatus(a.EvidenceStatus)
                        })
                        .OrderByDescending(a => a.DateRequested)
                        .ToList()
                };

            }

            var amendments = _apiClient.GetAmendmentsByURNAsync(urn, checkingWindowURL).GetAwaiter().GetResult();

            return  new AmendmentsListViewModel
            {
                Urn = urn,
                AmendmentList = amendments.Result
                    .Select(a => new Amendment
                    {
                        FirstName = a.Pupil.ForeName,
                        LastName = a.Pupil.LastName,
                        PupilId = a.Pupil.Id,
                        DateRequested = a.CreatedDate.DateTime,
                        ReferenceId = a.Reference,
                        Id = a.Id,
                        Status = a.Status,
                        EvidenceStatus = GetEvidenceStatus(a.EvidenceStatus)
                    })
                    .OrderByDescending(a => a.DateRequested)
                    .ToList()
            };
        }

        private EvidenceStatus GetEvidenceStatus(Rscd.Web.ApiClient.EvidenceStatus evidenceStatus)
        {
            switch (evidenceStatus)
            {
                case Rscd.Web.ApiClient.EvidenceStatus.Now:
                    return EvidenceStatus.Now;
                case Rscd.Web.ApiClient.EvidenceStatus.Later:
                    return EvidenceStatus.Later;
                case Rscd.Web.ApiClient.EvidenceStatus.NotRequired:
                    return EvidenceStatus.NotRequired;
                case Rscd.Web.ApiClient.EvidenceStatus.Unknown:
                default:
                    return EvidenceStatus.Unknown;

            }
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
                Pupil = new ApiClient.Pupil
                {
                    Id = amendment.PupilDetails.ID,
                    ForeName = amendment.PupilDetails.FirstName,
                    LastName = amendment.PupilDetails.LastName,
                    Gender = amendment.PupilDetails.Gender.ToApiGender(),
                    DateOfBirth = amendment.PupilDetails.DateOfBirth,
                    Age = amendment.PupilDetails.Age,
                    Upn = amendment.PupilDetails.UPN
                },
                Urn = amendment.URN,
                EvidenceStatus = amendment.EvidenceOption.ToApiEvidenceStatus(),
                EvidenceFolderName = amendment.EvidenceFolderName
            };
            // TODO: need to add other amendment types here
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


        public AmendmentViewModel GetAddPupilAmendmentViewModel(Guid id)
        {
            var amendment = _amendmentService.GetAddPupilAmendmentDetail(id);
            return new AmendmentViewModel
            {
                PupilViewModel = new PupilViewModel
                {
                    UPN = amendment.Pupil.Id?.Value,
                    FirstName = amendment.Pupil.ForeName,
                    LastName = amendment.Pupil.LastName,
                    DateOfBirth = amendment.Pupil.DateOfBirth,
                    Gender = amendment.Pupil.Gender == Domain.Core.Enums.Gender.Male ? Gender.Male : Gender.Female,
                    Age = amendment.Pupil.Age,
                    DateOfAdmission = amendment.Pupil.DateOfAdmission,
                    YearGroup = amendment.Pupil.YearGroup
                },
                Results = amendment.PriorAttainmentResults.Select(r => new PriorAttainmentResultViewModel() { Subject = GetSubject(r.Subject), ExamYear = r.ExamYear, TestMark = r.TestMark, ScaledScore = r.ScaledScore }).ToList(),

            };
        }

        public bool CancelAmendment(string id)
        {
            return _amendmentService.CancelAddPupilAmendment(new Guid(id));
        }

        public string UploadEvidence(IEnumerable<IFormFile> files)
        {
            var now = DateTime.UtcNow;
            var folderName = $"{now:yyyy-MM-dd-HH-mm-ss}-{Guid.NewGuid()}";
            var validFileReceived = false;

            foreach (var file in files)
            {
                if (file.Length == 0)
                {
                    continue;
                }

                validFileReceived = true;

                using (var fs = file.OpenReadStream())
                {
                    _fileUploadService.UploadFile(fs, file.FileName, file.ContentType, folderName);
                }
            }

            return validFileReceived ? folderName : null;
        }

        public void RelateEvidence(Guid amendmentId, string evidenceFolderName)
        {
            _amendmentService.RelateEvidence(amendmentId, evidenceFolderName, true);
        }

        public bool CreateAddPupilAmendment(AddPupilAmendmentViewModel addPupilAmendment, out string id)
        {
            var selectedEvidenceOption = addPupilAmendment.SelectedEvidenceOption;
            var result = _amendmentService.CreateAddPupilAmendment(new AddPupilAmendment
            {
                AddReason = addPupilAmendment.AddReason == AddReason.New ? Domain.Core.Enums.AddReason.New: Domain.Core.Enums.AddReason.Existing,
                Pupil = new Pupil
                {
                    Urn = new URN(addPupilAmendment.URN),
                    LaEstab = addPupilAmendment.AddReason == AddReason.Existing ? addPupilAmendment.PupilViewModel.SchoolID : null,
                    UPN = addPupilAmendment.AddReason == AddReason.Existing ? addPupilAmendment.PupilViewModel.UPN : null,
                    ForeName = addPupilAmendment.PupilViewModel.FirstName,
                    LastName = addPupilAmendment.PupilViewModel.LastName,
                    DateOfBirth = addPupilAmendment.PupilViewModel.DateOfBirth,
                    Age = addPupilAmendment.PupilViewModel.Age,
                    Gender = addPupilAmendment.PupilViewModel.Gender == Gender.Male
                        ? Domain.Core.Enums.Gender.Male
                        : Domain.Core.Enums.Gender.Female,
                    DateOfAdmission = addPupilAmendment.PupilViewModel.DateOfAdmission,
                    YearGroup = addPupilAmendment.PupilViewModel.YearGroup
                },
                InclusionConfirmed = true,
                PriorAttainmentResults = addPupilAmendment.Results.Select(r => new PriorAttainment{ Subject = GetSubject(r.Subject), ExamYear = r.ExamYear, TestMark = r.TestMark, ScaledScore = r.ScaledScore}).ToList(),
                EvidenceStatus = selectedEvidenceOption == Models.ViewModels.Pupil.EvidenceOption.UploadNow ?
                    EvidenceStatus.Now : selectedEvidenceOption == Models.ViewModels.Pupil.EvidenceOption.UploadLater ?
                        EvidenceStatus.Later : EvidenceStatus.NotRequired,
                EvidenceFolderName = addPupilAmendment.EvidenceFolderName
            }, out id);
            return result;
        }

        private Ks2Subject GetSubject(Domain.Core.Enums.Ks2Subject subject)
        {
            switch (subject)
            {
                case Domain.Core.Enums.Ks2Subject.Reading:
                    return Ks2Subject.Reading;
                case Domain.Core.Enums.Ks2Subject.Writing:
                    return Ks2Subject.Writing;
                case Domain.Core.Enums.Ks2Subject.Maths:
                    return Ks2Subject.Maths;
                default:
                    return Ks2Subject.Unknown;
            }
        }
        private Domain.Core.Enums.Ks2Subject GetSubject(Ks2Subject subject)
        {
            switch (subject)
            {
                case Ks2Subject.Reading:
                    return Domain.Core.Enums.Ks2Subject.Reading;
                case Ks2Subject.Writing:
                    return Domain.Core.Enums.Ks2Subject.Writing;
                case Ks2Subject.Maths:
                    return Domain.Core.Enums.Ks2Subject.Maths;
                default:
                    return Domain.Core.Enums.Ks2Subject.Unknown;
            }
        }
    }
}
