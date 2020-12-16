using System;
using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.AddPupil;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Dfe.CspdAlpha.Web.Application.Application;
using Dfe.Rscd.Web.ApiClient;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class AddPupilController : Controller
    {
        private readonly IPupilService _pupilService;
        private readonly IEstablishmentService _establishmentService;
        private CheckingWindow CheckingWindow => CheckingWindowHelper.GetCheckingWindow(RouteData);

        public AddPupilController(IPupilService pupilService, IEstablishmentService establishmentService)
        {
            _establishmentService = establishmentService;
            _pupilService = pupilService;
        }

        public IActionResult Index()
        {
            var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            if (amendment != null)
            {
                return View(new AddPupilViewModel
                {
                    UPN = amendment.Pupil.Upn,
                    FirstName = amendment.Pupil.ForeName,
                    LastName = amendment.Pupil.LastName,
                    Gender = amendment.Pupil.Gender,
                    DateOfBirth = new DateViewModel(amendment.Pupil.DateOfBirth.UtcDateTime),
                    DateOfAdmission = new DateViewModel(amendment.Pupil.DateOfAdmission.UtcDateTime),
                    YearGroup = amendment.Pupil.YearGroup,
                    SchoolID = amendment.Pupil.LaEstab
                });
            }
            return View(new AddPupilViewModel());
        }

        [HttpPost]
        public IActionResult Index(AddPupilViewModel addPupilViewModel, string urn)
        {
            MatchedPupilViewModel existingPupil = null;

            if (!string.IsNullOrEmpty(addPupilViewModel.UPN))
            {
                existingPupil = _pupilService.GetMatchedPupil(CheckingWindow, addPupilViewModel.UPN);
                if (existingPupil == null)
                {
                    ModelState.AddModelError(nameof(addPupilViewModel.UPN), "Enter a valid UPN");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(addPupilViewModel);
            }

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
                        ForeName = existingPupil.PupilViewModel.FirstName,
                        LastName = existingPupil.PupilViewModel.LastName,
                        Gender = existingPupil.PupilViewModel.Gender,
                        DateOfBirth = existingPupil.PupilViewModel.DateOfBirth,
                        DateOfAdmission = existingPupil.PupilViewModel.DateOfAdmission,
                        Age = existingPupil.PupilViewModel.Age,
                        Upn = existingPupil.PupilViewModel.UPN,
                        YearGroup = existingPupil.PupilViewModel.YearGroup,
                        LaEstab = existingPupil.PupilViewModel.SchoolID,
                        Urn = existingPupil.PupilViewModel.URN
                    },
                    AmendmentDetail = new AmendmentDetail()
                };

                amendment.AmendmentDetail.AddField("AddReason", AddReason.Existing);
                amendment.AmendmentDetail.AddField("PreviousSchoolLAEstab", existingPupil.PupilViewModel.SchoolID);
                amendment.AmendmentDetail.AddField("PreviousSchoolURN", existingPupil.PupilViewModel.URN);
                amendment.AmendmentDetail.AddField("PriorAttainmentResults", existingPupil.Results
                    .Select(r =>
                        new PriorAttainmentResult
                        {
                            Ks2Subject = r.Subject, ExamYear = r.ExamYear, Mark = r.TestMark,
                            ScaledScore = r.ScaledScore
                        })
                    .ToList());

                HttpContext.Session.Set(Constants.AMENDMENT_SESSION_KEY, amendment);

                return RedirectToAction("MatchedPupil");
            }

            var addPupilAmendment = new Amendment
            {
                Urn = urn,
                Pupil = new Pupil
                {
                    ForeName = addPupilViewModel.FirstName,
                    LastName = addPupilViewModel.LastName,
                    Gender = addPupilViewModel.Gender.Value,
                    DateOfBirth = addPupilViewModel.DateOfBirth.Date.Value,
                    Age = CalculateAge(addPupilViewModel.DateOfBirth.Date.Value),
                    DateOfAdmission = addPupilViewModel.DateOfAdmission.Date.Value,
                    Upn = addPupilViewModel.UPN,
                    YearGroup = addPupilViewModel.YearGroup,
                    LaEstab = addPupilViewModel.SchoolID,
                    Urn = urn
                },
                CheckingWindow = CheckingWindow,
                AmendmentDetail = new AmendmentDetail()
            };

            addPupilAmendment.AmendmentDetail.AddField("AddReason", AddReason.New);
            addPupilAmendment.AmendmentDetail.AddField("PriorAttainmentResults", new List<PriorAttainmentResult>());

            HttpContext.Session.Set(Constants.AMENDMENT_SESSION_KEY, addPupilAmendment);

            return RedirectToAction("Add", "PriorAttainment");
        }

        private int CalculateAge(DateTime dateOfBirth)
        {
            var age = DateTime.Now.Year - dateOfBirth.Year;
            return dateOfBirth.DayOfYear > DateTime.Now.DayOfYear ? age - 1 : age;
        }

        public IActionResult MatchedPupil(string urn)
        {
            var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            if (amendment.Pupil == null || amendment.AmendmentDetail.GetField<string>("AddReason") == AddReason.New)
            {
                return RedirectToAction("Index");
            }

            var existingSchoolId = amendment.AmendmentDetail.GetField<string>("PreviousSchoolLAEstab");
            var existingSchoolName = _establishmentService.GetSchoolName(CheckingWindow, existingSchoolId);

            return View(
                new MatchedAddPupilViewModel
                {
                    PupilViewModel = new PupilViewModel
                    {
                        FirstName = amendment.Pupil.ForeName,
                        LastName = amendment.Pupil.LastName,
                        DateOfBirth = amendment.Pupil.DateOfBirth.UtcDateTime,
                        Gender = amendment.Pupil.Gender,
                        Age = amendment.Pupil.Age,
                        URN = urn,
                        DateOfAdmission = amendment.Pupil.DateOfAdmission.UtcDateTime,
                        ID = amendment.Pupil.Id,
                        YearGroup = amendment.Pupil.YearGroup,
                        UPN = amendment.Pupil.Upn,
                        Keystage = CheckingWindowHelper.ToKeyStage(CheckingWindow),
                        SchoolID = existingSchoolId
                    },
                    SchoolName = existingSchoolName
                });
        }
    }
}
