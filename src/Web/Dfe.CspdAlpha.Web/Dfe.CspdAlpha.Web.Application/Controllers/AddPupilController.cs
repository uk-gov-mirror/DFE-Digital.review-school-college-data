using System;
using System.Collections.Generic;
using System.Linq;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.AddPupil;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Dfe.Rscd.Web.ApiClient;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class AddPupilController : SessionController
    {
        private readonly IEstablishmentService _establishmentService;
        private readonly IPupilService _pupilService;

        public AddPupilController(IPupilService pupilService, IEstablishmentService establishmentService)
        {
            _establishmentService = establishmentService;
            _pupilService = pupilService;
        }

        public IActionResult Index()
        {
            var amendment = GetAmendment();

            if (amendment != null)
                return View(new AddPupilViewModel
                {
                    UPN = amendment.Pupil.Upn,
                    FirstName = amendment.Pupil.Forename,
                    LastName = amendment.Pupil.Surname,
                    Gender = amendment.Pupil.Gender.Code,
                    DateOfBirth = new DateViewModel(amendment.Pupil.Dob.UtcDateTime),
                    DateOfAdmission = new DateViewModel(amendment.Pupil.Dob.UtcDateTime),
                    YearGroup = amendment.Pupil.YearGroup,
                    SchoolID = amendment.Pupil.DfesNumber
                });
            return View(new AddPupilViewModel());
        }

        [HttpPost]
        public IActionResult Index(AddPupilViewModel addPupilViewModel, string urn)
        {
            MatchedPupilViewModel existingPupil = null;

            if (!string.IsNullOrEmpty(addPupilViewModel.UPN))
            {
                existingPupil = _pupilService.GetMatchedPupil(addPupilViewModel.UPN);
                if (existingPupil == null) ModelState.AddModelError(nameof(addPupilViewModel.UPN), "Enter a valid UPN");
            }

            if (!ModelState.IsValid) return View(addPupilViewModel);

            if (existingPupil != null)
            {
                var amendment = new Amendment
                {
                    CheckingWindow = CheckingWindow,
                    Urn = urn,
                    AmendmentType = AmendmentType.AddPupil,
                    Pupil = new Pupil
                    {
                        Id = existingPupil.PupilViewModel.ID,
                        Forename = existingPupil.PupilViewModel.FirstName,
                        Surname = existingPupil.PupilViewModel.LastName,
                        Gender = existingPupil.PupilViewModel.Gender,
                        Dob = existingPupil.PupilViewModel.DateOfBirth,
                        AdmissionDate = existingPupil.PupilViewModel.DateOfAdmission,
                        Age = existingPupil.PupilViewModel.Age,
                        Upn = existingPupil.PupilViewModel.UPN,
                        YearGroup = existingPupil.PupilViewModel.YearGroup,
                        DfesNumber = existingPupil.PupilViewModel.SchoolID,
                        Urn = existingPupil.PupilViewModel.URN,
                        Pincl = new PInclude {Code = existingPupil.PupilViewModel.PincludeCode}
                    },
                    AmendmentDetail = new AmendmentDetail()
                };

                amendment.AmendmentDetail.AddField(Constants.AddPupil.AddReason, AddReason.Existing);
                amendment.AmendmentDetail.AddField(Constants.AddPupil.PreviousSchoolLAEstab,
                    existingPupil.PupilViewModel.SchoolID);
                amendment.AmendmentDetail.AddField(Constants.AddPupil.PreviousSchoolURN,
                    existingPupil.PupilViewModel.URN);
                amendment.AmendmentDetail.AddField(Constants.AddPupil.PriorAttainmentResults, existingPupil.Results
                    .Select(r =>
                        new PriorAttainmentResult
                        {
                            Ks2Subject = r.Subject, ExamYear = r.ExamYear, Mark = r.TestMark,
                            ScaledScore = r.ScaledScore
                        })
                    .ToList());

                SaveAmendment(amendment);

                return RedirectToAction("MatchedPupil");
            }

            var addPupilAmendment = new Amendment
            {
                Urn = urn,
                Pupil = new Pupil
                {
                    Forename = addPupilViewModel.FirstName,
                    Surname = addPupilViewModel.LastName,
                    Gender = Gender.FromCode(addPupilViewModel.Gender),
                    Dob = addPupilViewModel.DateOfBirth.Date.Value,
                    Age = CalculateAge(addPupilViewModel.DateOfBirth.Date.Value),
                    AdmissionDate = addPupilViewModel.DateOfAdmission.Date.Value,
                    Upn = addPupilViewModel.UPN,
                    YearGroup = addPupilViewModel.YearGroup,
                    DfesNumber = addPupilViewModel.SchoolID,
                    Urn = urn,
                    Pincl = new PInclude {Code = "499"}
                },
                CheckingWindow = CheckingWindow,
                AmendmentDetail = new AmendmentDetail(),
                IsNewAmendment = true
            };

            addPupilAmendment.AmendmentDetail.AddField(Constants.AddPupil.AddReason, AddReason.New);
            addPupilAmendment.AmendmentDetail.AddField(Constants.AddPupil.PriorAttainmentResults,
                new List<PriorAttainmentResult>());

            SaveAmendment(addPupilAmendment);

            return RedirectToAction("Add", "PriorAttainment");
        }

        private int CalculateAge(DateTime dateOfBirth)
        {
            var age = DateTime.Now.Year - dateOfBirth.Year;
            return dateOfBirth.DayOfYear > DateTime.Now.DayOfYear ? age - 1 : age;
        }

        public IActionResult MatchedPupil(string urn)
        {
            var amendment = GetAmendment();

            if (amendment.Pupil == null || amendment.AmendmentDetail.GetField<string>(Constants.AddPupil.AddReason) ==
                AddReason.New) return RedirectToAction("Index");

            var existingSchoolId = amendment.AmendmentDetail.GetField<string>(Constants.AddPupil.PreviousSchoolLAEstab);
            var existingSchoolName = _establishmentService.GetSchoolName(existingSchoolId);

            return View(
                new MatchedAddPupilViewModel
                {
                    PupilViewModel = new PupilViewModel
                    {
                        FirstName = amendment.Pupil.Forename,
                        LastName = amendment.Pupil.Surname,
                        DateOfBirth = amendment.Pupil.Dob.UtcDateTime,
                        Gender = amendment.Pupil.Gender,
                        Age = amendment.Pupil.Age,
                        URN = urn,
                        DateOfAdmission = amendment.Pupil.AdmissionDate.UtcDateTime,
                        ID = amendment.Pupil.Id,
                        YearGroup = amendment.Pupil.YearGroup,
                        UPN = amendment.Pupil.Upn,
                        Keystage = CheckingWindowHelper.ToKeyStage(CheckingWindow),
                        SchoolID = existingSchoolId,
                        PincludeCode = amendment.Pupil.Pincl != null ? amendment.Pupil.Pincl.Code : "499"
                    },
                    SchoolName = existingSchoolName
                });
        }
    }
}
