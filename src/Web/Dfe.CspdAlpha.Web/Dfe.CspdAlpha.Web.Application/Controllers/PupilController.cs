using System;
using Dfe.CspdAlpha.Web.Application.Application.Extensions;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Application.Application;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Microsoft.AspNetCore.Http;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    [TasksReviewedFilter("Index")]
    public class PupilController : Controller
    {
        private ISchoolService _schoolService;
        private readonly IAmendmentService _amendmentService;
        private const string ADD_PUPIL_AMENDMENT = "add-pupil-amendment";
        private const string ADD_PUPIL_AMENDMENT_ID = "add-pupil-amendment-id";

        public PupilController(
            ISchoolService schoolService,
            IAmendmentService amendmentService)
        {
            _schoolService = schoolService;
            _amendmentService = amendmentService;
        }
        public IActionResult Index(string urn)
        {
            var viewModel = _schoolService.GetPupilListViewModel(urn);
            return View(viewModel);
        }

        public IActionResult AddReason()
        {
            var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>(ADD_PUPIL_AMENDMENT);
            return View(addPupilAmendment != null ? addPupilAmendment.AddReasonViewModel : new AddReasonViewModel());
        }

        [HttpPost]
        public IActionResult AddReason(AddReasonViewModel addReasonViewModel)
        {
            if (ModelState.IsValid && addReasonViewModel.Reason != Models.Common.AddReason.Unknown)
            { 
                var addPupilAmendment = new AddPupilAmendmentViewModel { AddReasonViewModel = addReasonViewModel, URN = ClaimsHelper.GetURN(this.User), EvidenceFiles = new List<EvidenceFile>()};
                HttpContext.Session.Set(ADD_PUPIL_AMENDMENT, addPupilAmendment);
                return RedirectToAction("AddPupil");
            }
            return View(addReasonViewModel);
        }


        public IActionResult AddPupil()
        {
            var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>(ADD_PUPIL_AMENDMENT);
            if (addPupilAmendment == null || addPupilAmendment.AddReasonViewModel == null)
            {
                return RedirectToAction("AddReason");
            }
            return View(addPupilAmendment.AddPupilViewModel ?? new AddPupilViewModel{AddReason = addPupilAmendment.AddReasonViewModel.Reason });
        }

        [HttpPost]
        public IActionResult AddPupil(AddPupilViewModel addPupilViewModel)
        {
            var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>(ADD_PUPIL_AMENDMENT);
            addPupilAmendment.AddPupilViewModel = addPupilViewModel;
            if (ModelState.IsValid)
            {
                HttpContext.Session.Set(ADD_PUPIL_AMENDMENT, addPupilAmendment);
                return RedirectToAction("AddResult");
            }
            return View(addPupilViewModel);
        }

        public IActionResult AddResult()
        {
            var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>(ADD_PUPIL_AMENDMENT);
            if (addPupilAmendment == null || addPupilAmendment.AddPupilViewModel == null)
            {
                return RedirectToAction("AddReason");
            }
            return View(addPupilAmendment);
        }

        [HttpPost]
        public IActionResult AddResult(AddPriorAttainmentViewModel addPriorAttainmentViewModel)
        {
            var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>(ADD_PUPIL_AMENDMENT);
            addPupilAmendment.AddPriorAttainmentViewModel = addPriorAttainmentViewModel;
            if (ModelState.IsValid)
            {
                HttpContext.Session.Set(ADD_PUPIL_AMENDMENT, addPupilAmendment);
                return RedirectToAction("AddEvidence");
            }
            return View(addPupilAmendment);
        }

        public IActionResult AddEvidence()
        {
            var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>(ADD_PUPIL_AMENDMENT);
            if (addPupilAmendment?.AddPriorAttainmentViewModel == null)
            {
                return RedirectToAction("AddReason");
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
                    return RedirectToAction("AddReason");
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
                return RedirectToAction("AddReason");
            }
            return View(new ConfirmAddPupilViewModel{ AddPupilViewModel = addPupilAmendment.AddPupilViewModel, AddReasonViewModel = addPupilAmendment.AddReasonViewModel});
        }

        [HttpPost]
        public IActionResult ConfirmAddPupil(ConfirmAddPupilViewModel confirmAddPupilViewModel)
        {
            // Ensure steps hasn't been manually skipped
            var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>(ADD_PUPIL_AMENDMENT);
            if (addPupilAmendment == null || addPupilAmendment.SelectedEvidenceOption == EvidenceOption.Unknown)
            {
                return RedirectToAction("AddReason");
            }

            // Cancel amendment
            if (!confirmAddPupilViewModel.ConfirmAddPupil)
            {
                // Cancel amendment
                HttpContext.Session.Remove(ADD_PUPIL_AMENDMENT);
                return RedirectToAction("Index", "TaskList");
            }

            // Confirmation of new pupil add amendment before selection made
            if (addPupilAmendment.AddReasonViewModel.Reason == Models.Common.AddReason.New && string.IsNullOrEmpty(confirmAddPupilViewModel.SelectedPupilId))
            {
                var matchesPupils = _schoolService.GetMatchedPupils(addPupilAmendment.AddPupilViewModel);
                if (matchesPupils.Count == 0)
                {
                    return RedirectToAction("InclusionDetails");
                }
                return View(new ConfirmAddPupilViewModel { AddPupilViewModel = addPupilAmendment.AddPupilViewModel, AddReasonViewModel = addPupilAmendment.AddReasonViewModel, MatchedPupils = matchesPupils});
            }
            // Confirmation of new pupil add amendment with selection made
            if (addPupilAmendment.AddReasonViewModel.Reason == Models.Common.AddReason.New)
            {
                addPupilAmendment.ExistingMatchedPupil = confirmAddPupilViewModel.SelectedPupilId != "0";
                HttpContext.Session.Set(ADD_PUPIL_AMENDMENT, addPupilAmendment);
                return RedirectToAction("InclusionDetails");
            }
            // Confirmation of existing pupil add amendment
            if (addPupilAmendment.AddReasonViewModel.Reason == Models.Common.AddReason.Existing && !string.IsNullOrEmpty(addPupilAmendment.AddPupilViewModel.LastName))
            {
                return RedirectToAction("InclusionDetails");
            }

            return View(confirmAddPupilViewModel);
        }

        public IActionResult InclusionDetails()
        {
            var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>(ADD_PUPIL_AMENDMENT);
            if (addPupilAmendment == null || addPupilAmendment.SelectedEvidenceOption == EvidenceOption.Unknown)
            {
                return RedirectToAction("AddReason");
            }
            return View(addPupilAmendment);
        }

        [HttpPost]
        public IActionResult InclusionDetails(bool inclusionConfirmed)
        {
            var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>(ADD_PUPIL_AMENDMENT);
            addPupilAmendment.InclusionConfirmed = inclusionConfirmed;
            if (_amendmentService.CreateAddPupilAmendment(addPupilAmendment, out string id))
            {
                HttpContext.Session.Remove(ADD_PUPIL_AMENDMENT);
                HttpContext.Session.Set(ADD_PUPIL_AMENDMENT_ID, id);
                return RedirectToAction("AmendmentReceived");

            }
            return View(addPupilAmendment);
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
