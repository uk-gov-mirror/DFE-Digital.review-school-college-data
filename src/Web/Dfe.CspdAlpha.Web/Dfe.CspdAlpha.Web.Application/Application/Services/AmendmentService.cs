using System;
using System.Collections.Generic;
using System.Linq;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.School;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Results;
using Dfe.CspdAlpha.Web.Domain.Core;
using Dfe.CspdAlpha.Web.Domain.Core.Enums;
using Dfe.CspdAlpha.Web.Domain.Entities;
using Dfe.CspdAlpha.Web.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using AddReason = Dfe.CspdAlpha.Web.Application.Models.Common.AddReason;
using Gender = Dfe.CspdAlpha.Web.Application.Models.Common.Gender;
using DomainInterfaces = Dfe.CspdAlpha.Web.Domain.Interfaces;
using Ks2Subject = Dfe.CspdAlpha.Web.Application.Models.ViewModels.Results.Ks2Subject;

namespace Dfe.CspdAlpha.Web.Application.Application.Services
{
    public class AmendmentService : IAmendmentService
    {
        private DomainInterfaces.IAmendmentService _amendmentService;
        private IFileUploadService _fileUploadService;

        public AmendmentService(DomainInterfaces.IAmendmentService amendmentService, IFileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
            _amendmentService = amendmentService;
        }

        public AmendmentsListViewModel GetAmendmentsListViewModel(string urn, bool lateChecking = false)
        {
            var urnValue = new URN(urn);
            return new AmendmentsListViewModel
            {
                Urn = urn,
                AmendmentList = _amendmentService
                    .GetAddPupilAmendments(urnValue.Value)
                    .Where(a => !lateChecking || (a.Status == "Approved" || a.Status =="Rejected"))
                    .Select(a => new Amendment
                    {
                        FirstName = a.Pupil.ForeName,
                        LastName = a.Pupil.LastName,
                        PupilId = a.Pupil.Id?.Value,
                        DateRequested = a.CreatedDate,
                        ReferenceId = a.Reference,
                        Id = a.Id,
                        Status = a.Status,
                        EvidenceStatus = a.EvidenceStatus
                    })
                    .OrderByDescending(a => a.DateRequested)
                    .ToList()
            };
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
                    DayOfBirth = amendment.Pupil.DateOfBirth.Day,
                    MonthOfBirth = amendment.Pupil.DateOfBirth.Month,
                    YearOfBirth = amendment.Pupil.DateOfBirth.Year,
                    Gender = amendment.Pupil.Gender == Domain.Core.Enums.Gender.Male ? Gender.Male : Gender.Female,
                    DayOfAdmission = amendment.Pupil.DateOfAdmission.Day,
                    MonthOfAdmission = amendment.Pupil.DateOfAdmission.Month,
                    YearOfAdmission = amendment.Pupil.DateOfAdmission.Year,
                    YearGroup = amendment.Pupil.YearGroup
                },
                Results = amendment.PriorAttainmentResults.Select(r => new PriorAttainmentResultViewModel() { Subject = GetSubject(r.Subject), ExamYear = r.ExamYear, TestMark = r.TestMark, ScaledScore = r.ScaledScore }).ToList(),

            };
        }

        public bool CancelAmendment(string id)
        {
            return _amendmentService.CancelAddPupilAmendment(new Guid(id));
        }

        public List<EvidenceFile> UploadEvidence(List<IFormFile> files)
        {
            var fileIdList = new List<EvidenceFile>();
            var now = DateTime.UtcNow;
            var folderName = $"{now:yyyy-MM-dd-HH-mm-ss}-{Guid.NewGuid()}";

            foreach (var file in files)
            {
                using (var fs = file.OpenReadStream())
                {
                    if (fs.Length > 0)
                    {
                        var fileId = _fileUploadService.UploadFile(fs, file.FileName, file.ContentType, folderName);
                        fileIdList.Add(new EvidenceFile { Id = fileId.FileId.ToString(), Name = fileId.FileName });
                    }
                }

            }

            return fileIdList;
        }

        public void RelateEvidence(Guid amendmentId, List<EvidenceFile> evidenceList)
        {
            _amendmentService.RelateEvidence(amendmentId, evidenceList.Select(e => new Evidence{Id = e.Id, Name = e.Name}).ToList(), true);
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
                    Gender = addPupilAmendment.PupilViewModel.Gender == Gender.Male
                        ? Domain.Core.Enums.Gender.Male
                        : Domain.Core.Enums.Gender.Female,
                    DateOfAdmission = addPupilAmendment.PupilViewModel.DateOfAdmission,
                    YearGroup = addPupilAmendment.PupilViewModel.YearGroup
                },
                InclusionConfirmed = true,
                PriorAttainmentResults = addPupilAmendment.Results.Select(r => new PriorAttainment{ Subject = GetSubject(r.Subject), ExamYear = r.ExamYear, TestMark = r.TestMark, ScaledScore = r.ScaledScore}).ToList(),
                EvidenceStatus = selectedEvidenceOption == EvidenceOption.UploadNow ?
                    EvidenceStatus.Now : selectedEvidenceOption == EvidenceOption.UploadLater ?
                        EvidenceStatus.Later : EvidenceStatus.NotRequired,
                EvidenceList = addPupilAmendment.EvidenceFiles.Any() ? addPupilAmendment.EvidenceFiles.Select(e => new Evidence { Id = e.Id, Name = e.Name }).ToList() : new List<Evidence>()
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
