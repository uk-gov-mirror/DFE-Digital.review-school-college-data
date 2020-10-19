using System;
using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Application.Application.Extensions;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.Amendments;
using Dfe.CspdAlpha.Web.Application.Models.Amendments.AmendmentTypes;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.AddPupil;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class AddPupilController : Controller
    {
        private IPupilService _pupilService;
        private IEstablishmentService _establishmentService;
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
                    UPN = amendment.PupilDetails.UPN,
                    FirstName = amendment.PupilDetails.FirstName,
                    LastName = amendment.PupilDetails.LastName,
                    Gender = amendment.PupilDetails.Gender,
                    DateOfBirth = new DateViewModel(amendment.PupilDetails.DateOfBirth),
                    DateOfAdmission = new DateViewModel(amendment.PupilDetails.DateOfAdmission),
                    YearGroup = amendment.PupilDetails.YearGroup,
                    SchoolID = amendment.PupilDetails.LAEstab
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
                    URN = urn,
                    PupilDetails = new PupilDetails
                    {
                        ID = existingPupil.PupilViewModel.ID,
                        FirstName = existingPupil.PupilViewModel.FirstName,
                        LastName = existingPupil.PupilViewModel.LastName,
                        Gender = existingPupil.PupilViewModel.Gender,
                        DateOfBirth = existingPupil.PupilViewModel.DateOfBirth,
                        DateOfAdmission = existingPupil.PupilViewModel.DateOfAdmission,
                        Age = existingPupil.PupilViewModel.Age,
                        UPN = existingPupil.PupilViewModel.UPN,
                        YearGroup = existingPupil.PupilViewModel.YearGroup,
                        Keystage = existingPupil.PupilViewModel.Keystage,
                        LAEstab = existingPupil.PupilViewModel.SchoolID,
                        URN = existingPupil.PupilViewModel.URN
                    },
                    AmendmentDetail = new AddPupil
                    {
                        AddReason = AddReason.Existing,
                        PreviousSchoolLAEstab = existingPupil.PupilViewModel.SchoolID,
                        PreviousSchoolURN = existingPupil.PupilViewModel.URN,
                        PriorAttainmentResults = existingPupil.Results.Select(r => new PriorAttainmentResult
                            { Ks2Subject = r.Subject, ExamYear = r.ExamYear, Mark = r.TestMark, ScaledScore = r.ScaledScore }).ToList()
                    }
                };

                HttpContext.Session.Set(Constants.AMENDMENT_SESSION_KEY, amendment);

                return RedirectToAction("MatchedPupil");
            }

            var addPupilAmendment = new Amendment
            {
                URN = urn,
                PupilDetails = new PupilDetails
                {
                    FirstName = addPupilViewModel.FirstName,
                    LastName = addPupilViewModel.LastName,
                    Gender = addPupilViewModel.Gender.Value,
                    DateOfBirth = addPupilViewModel.DateOfBirth.Date.Value,
                    Age = CalculateAge(addPupilViewModel.DateOfBirth.Date.Value),
                    DateOfAdmission = addPupilViewModel.DateOfAdmission.Date.Value,
                    UPN = addPupilViewModel.UPN,
                    YearGroup = addPupilViewModel.YearGroup,
                    Keystage = CheckingWindow.ToKeyStage(),
                    LAEstab = addPupilViewModel.SchoolID,
                    URN = urn
                },
                CheckingWindow = CheckingWindow,
                AmendmentDetail = new AddPupil
                {
                    AddReason = AddReason.New,
                    PriorAttainmentResults = new List<PriorAttainmentResult>()
                }
            };

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
            if (amendment.PupilDetails == null || ((AddPupil)amendment.AmendmentDetail).AddReason == AddReason.New)
            {
                return RedirectToAction("Index");
            }

            var existingSchoolId = ((AddPupil) amendment.AmendmentDetail).PreviousSchoolLAEstab;
            var existingSchoolName = _establishmentService.GetSchoolName(CheckingWindow, existingSchoolId);

            return View(
                new MatchedAddPupilViewModel
                {
                    PupilViewModel = new PupilViewModel
                    {
                        FirstName = amendment.PupilDetails.FirstName,
                        LastName = amendment.PupilDetails.LastName,
                        DateOfBirth = amendment.PupilDetails.DateOfBirth,
                        Gender = amendment.PupilDetails.Gender,
                        Age = amendment.PupilDetails.Age,
                        URN = urn,
                        DateOfAdmission = amendment.PupilDetails.DateOfAdmission,
                        ID = amendment.PupilDetails.ID,
                        YearGroup = amendment.PupilDetails.YearGroup,
                        UPN = amendment.PupilDetails.UPN,
                        Keystage = amendment.PupilDetails.Keystage,
                        SchoolID = existingSchoolId
                    },
                    SchoolName = existingSchoolName
                });
        }
    }
}
