using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.School;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Dfe.CspdAlpha.Web.Domain.Core;
using Dfe.CspdAlpha.Web.Domain.Core.Enums;
using Dfe.CspdAlpha.Web.Domain.Entities;
using Dfe.CspdAlpha.Web.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Xrm.Sdk;
using AddReason = Dfe.CspdAlpha.Web.Application.Models.Common.AddReason;
using Gender = Dfe.CspdAlpha.Web.Application.Models.Common.Gender;
using DomainInterfaces = Dfe.CspdAlpha.Web.Domain.Interfaces;

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

        public AmendmentsListViewModel GetAmendmentsListViewModel(string urn)
        {
            var urnValue = new URN(urn);
            return new AmendmentsListViewModel
            {
                Urn = urn,
                AmendmentList = _amendmentService
                    .GetAddPupilAmendments(urnValue.Value)
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
                AddPupilViewModel = new AddPupilViewModel
                {
                    PupilId = amendment.Pupil.Id?.Value,
                    FirstName = amendment.Pupil.ForeName,
                    LastName = amendment.Pupil.LastName,
                    DayOfBirth = amendment.Pupil.DateOfBirth.Day,
                    MonthOfBirth = amendment.Pupil.DateOfBirth.Month,
                    YearOfBirth = amendment.Pupil.DateOfBirth.Year,
                    Gender = amendment.Pupil.Gender == Domain.Core.Enums.Gender.Male ? Gender.Male : Gender.Female,
                    DayOfAdmission = amendment.Pupil.DateOfAdmission.Day,
                    MonthOfAdmission = amendment.Pupil.DateOfAdmission.Month,
                    YearOfAdmission = amendment.Pupil.DateOfAdmission.Year,
                    YearGroup = amendment.Pupil.YearGroup,
                    PostCode = amendment.Pupil.PostCode
                },
                AddPriorAttainmentViewModel = new AddPriorAttainmentViewModel
                {
                    ResultFor = amendment.PriorAttainment.ResultFor,
                    Test = amendment.PriorAttainment.Test,
                    Level = amendment.PriorAttainment.AttainmentLevel,
                    AcademicYear = amendment.PriorAttainment.AcademicYear
                }
            };
        }

        public bool CancelAmendment(string id)
        {
            return _amendmentService.CancelAddPupilAmendment(new Guid(id));
        }

        public List<EvidenceFile> UploadEvidence(List<IFormFile> files)
        {
            var fileIdList = new List<EvidenceFile>();
            foreach (var file in files)
            {
                using (var fs = file.OpenReadStream())
                {
                    if (fs.Length > 0)
                    {
                        var fileId = _fileUploadService.UploadFile(fs, file.FileName, file.ContentType);
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
                AddReason = addPupilAmendment.AddReasonViewModel.Reason == AddReason.New ? Domain.Core.Enums.AddReason.New: Domain.Core.Enums.AddReason.Existing,
                Pupil = new Domain.Entities.Pupil
                {
                    Urn = new URN(addPupilAmendment.URN),
                    LaEstab = addPupilAmendment.LaEstab,
                    Id = addPupilAmendment.AddPupilViewModel.PupilId != null ? new PupilId(addPupilAmendment.AddPupilViewModel.PupilId) : null,
                    ForeName = addPupilAmendment.AddPupilViewModel.FirstName,
                    LastName = addPupilAmendment.AddPupilViewModel.LastName,
                    DateOfBirth = addPupilAmendment.AddPupilViewModel.DateOfBirth,
                    Gender = addPupilAmendment.AddPupilViewModel.Gender == Gender.Male
                        ? Domain.Core.Enums.Gender.Male
                        : Domain.Core.Enums.Gender.Female,
                    DateOfAdmission = addPupilAmendment.AddPupilViewModel.DateOfAdmission,
                    YearGroup = addPupilAmendment.AddPupilViewModel.YearGroup,
                    PostCode = addPupilAmendment.AddPupilViewModel.PostCode
                },
                InclusionConfirmed = addPupilAmendment.InclusionConfirmed,
                PriorAttainment = new PriorAttainment
                {
                    ResultFor = addPupilAmendment.AddPriorAttainmentViewModel.ResultFor,
                    Test = addPupilAmendment.AddPriorAttainmentViewModel.Test,
                    AcademicYear = addPupilAmendment.AddPriorAttainmentViewModel.AcademicYear,
                    AttainmentLevel = addPupilAmendment.AddPriorAttainmentViewModel.Level,
                },
                EvidenceStatus = selectedEvidenceOption == EvidenceOption.UploadNow ?
                    EvidenceStatus.Now : selectedEvidenceOption == EvidenceOption.UploadLater ?
                        EvidenceStatus.Later : EvidenceStatus.NotRequired,
                EvidenceList = addPupilAmendment.EvidenceFiles.Any() ? addPupilAmendment.EvidenceFiles.Select(e => new Evidence { Id = e.Id, Name = e.Name }).ToList() : new List<Evidence>()
            }, out id);
            return result;
        }
    }
}
