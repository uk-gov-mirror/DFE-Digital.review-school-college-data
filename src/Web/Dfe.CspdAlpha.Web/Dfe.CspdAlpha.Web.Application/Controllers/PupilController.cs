using System;
using Dfe.CspdAlpha.Web.Application.Application.Extensions;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Application.Application;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    [TasksReviewedFilter("Index")]
    public class PupilController : Controller
    {
        private ISchoolService _schoolService;
        private readonly IAmendmentService _amendmentService;
        private const string ADD_PUPIL_AMENDMENT = "add-pupil-amendment";
        private const string ADD_PUPIL_AMENDMENT_ID = "add-pupil-amendment-id";
        private bool LateChecking { get; }

        public PupilController(
            ISchoolService schoolService,
            IAmendmentService amendmentService,
            IConfiguration configuration)
        {
            _schoolService = schoolService;
            _amendmentService = amendmentService;
            LateChecking = configuration["CheckingPhase"] == "Late";
        }
        public IActionResult Index(string urn)
        {
            var viewModel = _schoolService.GetPupilListViewModel(urn);
            viewModel.LateCheckingPhase = LateChecking;
            return View(viewModel);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddPupilViewModel addPupilViewModel)
        {
            if (!string.IsNullOrEmpty(addPupilViewModel.UPN))
            {
                var existingPupil = _schoolService.GetMatchedPupil(addPupilViewModel.UPN);
                if (existingPupil == null)
                {
                    ModelState.AddModelError("UPN", "Enter a valid UPN");
                }
                else
                {
                    addPupilViewModel.SchoolID = existingPupil.LaEstab;
                    addPupilViewModel.FirstName = existingPupil.FirstName;
                    addPupilViewModel.LastName = existingPupil.LastName;
                    addPupilViewModel.DateOfBirth = existingPupil.DateOfBirth;
                    addPupilViewModel.Gender = existingPupil.Gender;
                    addPupilViewModel.DateOfAdmission = existingPupil.DateOfAdmission;
                    addPupilViewModel.YearGroup = existingPupil.YearGroup;
                    addPupilViewModel.PostCode = existingPupil.PostCode;
                    var addPupilAmendment = new AddPupilAmendmentViewModel
                    {
                        URN = ClaimsHelper.GetURN(this.User),
                        AddPupilViewModel = addPupilViewModel,
                        Results = new List<PriorAttainmentResultViewModel>(), //TODO: fill this out
                        EvidenceFiles = new List<EvidenceFile>()
                    };
                    HttpContext.Session.Set(ADD_PUPIL_AMENDMENT, addPupilAmendment);
                    return RedirectToAction("ExistingMatch");
                }
            }

            if (ModelState.IsValid)
            {
                var addPupilAmendment = new AddPupilAmendmentViewModel
                {
                    URN = ClaimsHelper.GetURN(this.User),
                    AddPupilViewModel = addPupilViewModel,
                    Results = new List<PriorAttainmentResultViewModel>(),
                    EvidenceFiles = new List<EvidenceFile>()
                };
                HttpContext.Session.Set(ADD_PUPIL_AMENDMENT, addPupilAmendment);
                return RedirectToAction("Add", "PriorAttainment");
            }

            return View(addPupilViewModel);
        }

        public IActionResult ExistingMatch()
        {
            var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>(ADD_PUPIL_AMENDMENT);
            if (addPupilAmendment.AddPupilViewModel == null)
            {
                return RedirectToAction("Add");
            }

            var school = _schoolService.GetSchoolName(addPupilAmendment.AddPupilViewModel.SchoolID);
            return View(new ExistingMatchViewModel{AddPupilViewModel = addPupilAmendment.AddPupilViewModel, SchoolName = school});
        }

        public IActionResult AddEvidence()
        {
            var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>(ADD_PUPIL_AMENDMENT);
            if (addPupilAmendment.AddPupilViewModel == null)
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
                return View(new UploadEvidenceViewModel{ AddPupilViewModel = addPupilAmendment.AddPupilViewModel });
            }
            else
            {
                var addPupilAmendment = _amendmentService.GetAddPupilAmendmentViewModel(new Guid(id));
                return View(new UploadEvidenceViewModel { AddPupilViewModel = addPupilAmendment.AddPupilViewModel, Id = id });
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
                    var uploadedEvidenceFiles = _amendmentService.UploadEvidence(evidenceFiles);
                    addPupilAmendment.EvidenceFiles.AddRange(uploadedEvidenceFiles);
                    HttpContext.Session.Set(ADD_PUPIL_AMENDMENT, addPupilAmendment);
                    return RedirectToAction("ConfirmAddPupil");
                }
                return View(new UploadEvidenceViewModel { AddPupilViewModel = addPupilAmendment.AddPupilViewModel });
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
                return View(new UploadEvidenceViewModel { AddPupilViewModel = addPupilAmendment.AddPupilViewModel, Id = id });
            }
        }

        public IActionResult ConfirmAddPupil()
        {
            var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>(ADD_PUPIL_AMENDMENT);
            if (addPupilAmendment == null || addPupilAmendment.SelectedEvidenceOption == EvidenceOption.Unknown)
            {
                return RedirectToAction("Add");
            }
            
            return View(new ConfirmAddPupilViewModel{ AddPupilViewModel = addPupilAmendment.AddPupilViewModel, SelectedEvidenceOption = addPupilAmendment.SelectedEvidenceOption});
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
            //// Confirmation of new pupil add amendment before selection made
            //if (addPupilAmendment.AddReasonViewModel.Reason == Models.Common.AddReason.New && string.IsNullOrEmpty(confirmAddPupilViewModel.SelectedPupilId))
            //{
            //    var matchesPupils = _schoolService.GetMatchedPupils(addPupilAmendment.AddPupilViewModel);
            //    if (matchesPupils.Count == 0)
            //    {
            //        return RedirectToAction("InclusionDetails");
            //    }
            //    return View(new ConfirmAddPupilViewModel
            //    {
            //        AddPupilViewModel = addPupilAmendment.AddPupilViewModel,
            //        AddReasonViewModel = addPupilAmendment.AddReasonViewModel,
            //        MatchedPupils = matchesPupils, SelectedEvidenceOption = addPupilAmendment.SelectedEvidenceOption,
            //        MatchedPupilCount = matchesPupils.Count
            //    });
            //}

            //// Confirmation of new pupil add amendment with selection made
            //if (addPupilAmendment.AddReasonViewModel.Reason == Models.Common.AddReason.New)
            //{
            //    addPupilAmendment.MatchedPupilCount = confirmAddPupilViewModel.MatchedPupilCount;
            //    addPupilAmendment.ExistingMatchedPupil = confirmAddPupilViewModel.SelectedPupilId != "0" ? confirmAddPupilViewModel.SelectedPupilId : string.Empty;
            //    HttpContext.Session.Set(ADD_PUPIL_AMENDMENT, addPupilAmendment);
            //    return RedirectToAction("InclusionDetails");
            //}
            //// Confirmation of existing pupil add amendment
            //if (addPupilAmendment.AddReasonViewModel.Reason == Models.Common.AddReason.Existing && !string.IsNullOrEmpty(addPupilAmendment.AddPupilViewModel.LAEstab))
            //{
            //    return RedirectToAction("InclusionDetails");
            //}

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
    }
}
