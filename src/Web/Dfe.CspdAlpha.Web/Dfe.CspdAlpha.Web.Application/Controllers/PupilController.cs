using Dfe.CspdAlpha.Web.Application.Application;
using Dfe.CspdAlpha.Web.Application.Application.Extensions;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Results;
using Dfe.CspdAlpha.Web.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    [TasksReviewedFilter("Index,View")]
    public class PupilController : Controller
    {
        private ISchoolService _schoolService;
        private readonly IAmendmentService _amendmentService;
        private const string ADD_PUPIL_AMENDMENT = "add-pupil-amendment";
        private const string ADD_PUPIL_AMENDMENT_ID = "add-pupil-amendment-id";

        public PupilController(
            ISchoolService schoolService,
            IAmendmentService amendmentService
            )
        {
            _schoolService = schoolService;
            _amendmentService = amendmentService;
        }

        public IActionResult Index(string urn)
        {
            var viewModel = _schoolService.GetPupilListViewModel(RouteData.Values["phase"].ToString(), urn);
            viewModel.CheckingWindow = CheckingWindowHelper.GetCheckingWindow(RouteData);
            return View(viewModel);
        }

        public IActionResult View(string id)
        {
            var viewModel = _schoolService.GetPupil(RouteData.Values["phase"].ToString(), id);
            return View(viewModel);
        }

        public IActionResult Add()
        {
            var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>(ADD_PUPIL_AMENDMENT);

            return View(
                addPupilAmendment == null
                ? null : MapToAddPupilDetailsViewModel(addPupilAmendment?.PupilViewModel));
        }

        [HttpPost]
        public IActionResult Add(AddPupilDetailsViewModel addPupilDetailsViewModel)
        {
            MatchedPupilViewModel existingPupil = null;

            if (!string.IsNullOrEmpty(addPupilDetailsViewModel.UPN))
            {
                existingPupil = _schoolService.GetMatchedPupil(
                    RouteData.Values["phase"].ToString(), addPupilDetailsViewModel.UPN);

                if (existingPupil == null)
                {
                    ModelState.AddModelError(nameof(addPupilDetailsViewModel.UPN), "Enter a valid UPN");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(addPupilDetailsViewModel);
            }

            if (existingPupil != null)
            {
                var amendment = new AddPupilAmendmentViewModel
                {
                    URN = ClaimsHelper.GetURN(this.User),
                    PupilViewModel = existingPupil.PupilViewModel,
                    Results = existingPupil.Results
                };

                HttpContext.Session.Set(ADD_PUPIL_AMENDMENT, amendment);

                return RedirectToAction("ExistingMatch");
            }

            var addPupilAmendment = new AddPupilAmendmentViewModel
            {
                URN = ClaimsHelper.GetURN(this.User),
                PupilViewModel = MapToPupilViewModel(addPupilDetailsViewModel),
                Results = new List<PriorAttainmentResultViewModel>()
            };

            HttpContext.Session.Set(ADD_PUPIL_AMENDMENT, addPupilAmendment);

            return RedirectToAction("Add", "PriorAttainment");
        }

        public IActionResult ExistingMatch()
        {
            var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>(ADD_PUPIL_AMENDMENT);
            if (addPupilAmendment.PupilViewModel == null)
            {
                return RedirectToAction("Add");
            }

            var school = _schoolService.GetSchoolName(addPupilAmendment.PupilViewModel.SchoolID);

            return View(
                new ExistingMatchViewModel
                {
                    PupilViewModel = addPupilAmendment.PupilViewModel,
                    SchoolName = school
                });
        }

        public IActionResult AddEvidence()
        {
            var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>(ADD_PUPIL_AMENDMENT);
            if (addPupilAmendment.PupilViewModel == null)
            {
                return RedirectToAction("Add");
            }
            return View(addPupilAmendment);
        }

        [HttpPost]
        public IActionResult AddEvidence(EvidenceOption selectedEvidenceOption)
        {
            var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>(ADD_PUPIL_AMENDMENT);
            addPupilAmendment.SelectedEvidenceOption = selectedEvidenceOption;
            HttpContext.Session.Set(ADD_PUPIL_AMENDMENT, addPupilAmendment);
            if (ModelState.IsValid)
            {
                switch (selectedEvidenceOption)
                {
                    case EvidenceOption.UploadNow:
                        return RedirectToAction("UploadEvidence");
                    case EvidenceOption.UploadLater:
                    case EvidenceOption.NotRequired:
                        return RedirectToAction("ConfirmAddPupil");
                    default:
                        return View(addPupilAmendment);
                }
            }
            return View(addPupilAmendment);
        }

        public IActionResult UploadEvidence(string id)
        {
            if (id == null)
            {
                var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>(ADD_PUPIL_AMENDMENT);

                if (addPupilAmendment == null || addPupilAmendment.SelectedEvidenceOption != EvidenceOption.UploadNow)
                {
                    return RedirectToAction("Add");
                }
                return View(new UploadEvidenceViewModel { PupilViewModel = addPupilAmendment.PupilViewModel });
            }
            else
            {
                var addPupilAmendment = _amendmentService.GetAddPupilAmendmentViewModel(new Guid(id));
                return View(new UploadEvidenceViewModel { PupilViewModel = addPupilAmendment.PupilViewModel, Id = id });
            }
        }

        [HttpPost]
        public IActionResult UploadEvidence(List<IFormFile> evidenceFiles, string id)
        {
            if (id == null)
            {
                var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>(ADD_PUPIL_AMENDMENT);

                if (ModelState.IsValid)
                {
                    addPupilAmendment.EvidenceFolderName = _amendmentService.UploadEvidence(evidenceFiles);
                    HttpContext.Session.Set(ADD_PUPIL_AMENDMENT, addPupilAmendment);

                    return RedirectToAction("ConfirmAddPupil");
                }

                return View(new UploadEvidenceViewModel { PupilViewModel = addPupilAmendment.PupilViewModel });
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var uploadedEvidenceFiles = _amendmentService.UploadEvidence(evidenceFiles);
                    _amendmentService.RelateEvidence(new Guid(id), uploadedEvidenceFiles);
                    return RedirectToAction("Index", "Amendments");
                }
                var addPupilAmendment = _amendmentService.GetAddPupilAmendmentViewModel(new Guid(id));
                return View(new UploadEvidenceViewModel { PupilViewModel = addPupilAmendment.PupilViewModel, Id = id });
            }
        }

        public IActionResult ConfirmAddPupil()
        {
            var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>(ADD_PUPIL_AMENDMENT);
            if (addPupilAmendment == null || addPupilAmendment.SelectedEvidenceOption == EvidenceOption.Unknown)
            {
                return RedirectToAction("Add");
            }

            return View(
                new ConfirmAddPupilViewModel
                {
                    PupilViewModel = addPupilAmendment.PupilViewModel,
                    SelectedEvidenceOption = addPupilAmendment.SelectedEvidenceOption
                });
        }

        [HttpPost]
        public IActionResult ConfirmAddPupil(ConfirmAddPupilViewModel confirmAddPupilViewModel)
        {
            // Ensure steps haven't been manually skipped
            var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>(ADD_PUPIL_AMENDMENT);
            if (addPupilAmendment == null || addPupilAmendment.SelectedEvidenceOption == EvidenceOption.Unknown)
            {
                return RedirectToAction("Add");
            }

            // Cancel amendment
            if (!confirmAddPupilViewModel.ConfirmAddPupil)
            {
                // Cancel amendment
                HttpContext.Session.Remove(ADD_PUPIL_AMENDMENT);
                return RedirectToAction("Index", "TaskList");
            }

            // Create amendment and redirect to amendment received page
            if (_amendmentService.CreateAddPupilAmendment(addPupilAmendment, out string id))
            {
                HttpContext.Session.Remove(ADD_PUPIL_AMENDMENT);
                HttpContext.Session.Set(ADD_PUPIL_AMENDMENT_ID, id);
                return RedirectToAction("AmendmentReceived");
            }

            confirmAddPupilViewModel.SelectedEvidenceOption = addPupilAmendment.SelectedEvidenceOption;
            return View(confirmAddPupilViewModel);
        }

        public IActionResult AmendmentReceived()
        {
            var addPupilAmendmentId = HttpContext.Session.Get<string>(ADD_PUPIL_AMENDMENT_ID);
            return View("AmendmentReceived", addPupilAmendmentId);
        }

        public IActionResult CancelAmendment()
        {
            HttpContext.Session.Remove(ADD_PUPIL_AMENDMENT);
            return RedirectToAction("Index");
        }

        // TODO: Remove as part of refactoring of view models (each view model should exist to serve the needs of a single view)
        private PupilViewModel MapToPupilViewModel(AddPupilDetailsViewModel addPupilDetails)
        {
            return new PupilViewModel
            {
                SchoolID = addPupilDetails.SchoolID,
                UPN = addPupilDetails.UPN,
                FirstName = addPupilDetails.FirstName,
                LastName = addPupilDetails.LastName,
                DateOfBirth = addPupilDetails.DateOfBirth.Date.Value,
                Gender = addPupilDetails.Gender.Value,
                DateOfAdmission = addPupilDetails.DateOfAdmission.Date.Value,
                YearGroup = addPupilDetails.YearGroup
            };
        }

        // TODO: Remove as part of refactoring of view models (each view model should exist to serve the needs of a single view)
        private AddPupilDetailsViewModel MapToAddPupilDetailsViewModel(PupilViewModel pupil)
        {
            return new AddPupilDetailsViewModel
            {
                SchoolID = pupil.SchoolID,
                UPN = pupil.UPN,
                FirstName = pupil.FirstName,
                LastName = pupil.LastName,
                DateOfBirth = new DateViewModel(pupil.DateOfBirth.Date),
                Gender = pupil.Gender,
                DateOfAdmission = new DateViewModel(pupil.DateOfAdmission.Date),
                YearGroup = pupil.YearGroup
            };
        }
    }
}
