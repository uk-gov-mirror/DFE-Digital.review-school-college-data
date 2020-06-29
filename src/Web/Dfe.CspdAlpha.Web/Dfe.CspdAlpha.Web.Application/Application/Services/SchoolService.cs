using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.School;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Dfe.CspdAlpha.Web.Domain.Core;
using Dfe.CspdAlpha.Web.Domain.Core.Enums;
using Dfe.CspdAlpha.Web.Domain.Entities;
using Dfe.CspdAlpha.Web.Domain.Interfaces;
using Dfe.CspdAlpha.Web.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.StaticFiles;
using Gender = Dfe.CspdAlpha.Web.Application.Models.Common.Gender;
using Pupil = Dfe.CspdAlpha.Web.Application.Models.School.Pupil;

namespace Dfe.CspdAlpha.Web.Application.Application.Services
{
    public class SchoolService : ISchoolService
    {
        private IEstablishmentService _establishmentService;
        private readonly List<string> HEADLINE_MEASURES = new List<string> { "P8_BANDING", "PTEBACC_E_PTQ_EE", "PTL2BASICS_95" };
        private readonly List<string> ADDITIONAL_MEASURES = new List<string> { "PTL2BASICS_94", "ATT8SCR", "EBACCAPS" };
        private IPupilService _pupilService;
        private IFileUploadService _fileUploadService;
        private IAmendmentService _amendmentService;

        public SchoolService(IEstablishmentService establishmentService, IPupilService pupilService, IFileUploadService fileUploadService, IAmendmentService amendmentService)
        {
            _amendmentService = amendmentService;
            _fileUploadService = fileUploadService;
            _pupilService = pupilService;
            _establishmentService = establishmentService;
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
                    URN = urn
                },
                HeadlineMeasures = establishmentData.PerformanceMeasures.Where(p => HEADLINE_MEASURES.Any(h => h == p.Name)).Select(m => new Measure{Name = m.Name, Data = m.Value}).ToList(),
                AdditionalMeasures = establishmentData.PerformanceMeasures.Where(p => ADDITIONAL_MEASURES.Any(h => h == p.Name)).Select(m => new Measure{Name = m.Name, Data = m.Value}).ToList(),
                CohortMeasures = establishmentData.CohortMeasures.Select(m => new Measure{Name = m.Name, Data = m.Value}).ToList()
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
                    .Select(p => new Pupil {FirstName = p.ForeName, LastName = p.LastName, PupilId = p.Id.Value})
                    .OrderBy(p => p.FirstName)
                    .ToList()
            };
        }

        public List<EvidenceFile> UploadEvidence(List<IFormFile> files)
        {
            var fileIdList = new List<EvidenceFile>();
            foreach (var file in files)
            {
                using (var fs = file.OpenReadStream())
                {
                    var fileId = _fileUploadService.UploadFile(fs, file.FileName, file.ContentType);
                    fileIdList.Add(new EvidenceFile{ Id = fileId.FileId.ToString(), Name = fileId.FileName});
                }

            }

            return fileIdList;
        }

        public bool CreateAddPupilAmendment(AddPupilAmendmentViewModel addPupilAmendment, out string id)
        {
            var selectedEvidenceOption = addPupilAmendment.SelectedEvidenceOption;
            var result = _amendmentService.CreateAddPupilAmendment(new AddPupilAmendment
            {
                AddReason = addPupilAmendment.AddReasonViewModel.Reason,
                Pupil = new Domain.Entities.Pupil
                {
                    LaEstab = addPupilAmendment.LaEstab,
                    Id = new PupilId(addPupilAmendment.AddPupilViewModel.PupilId),
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
                EvidenceList = addPupilAmendment.EvidenceFiles.Any() ? addPupilAmendment.EvidenceFiles.Select(e => new Evidence{Id = e.Id, Name = e.Name}).ToList() : new List<Evidence>()
            }, out id);
            return result;
        }
    }
}
