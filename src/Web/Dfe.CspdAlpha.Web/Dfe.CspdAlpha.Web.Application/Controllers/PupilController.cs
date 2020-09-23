using Dfe.CspdAlpha.Web.Application.Application;
using Dfe.CspdAlpha.Web.Application.Application.Extensions;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

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
            return View(addPupilAmendment?.PupilViewModel);
        }

        [HttpPost]
        public IActionResult Add(PupilViewModel addPupilViewModel)
        {
            if (!string.IsNullOrEmpty(addPupilViewModel.UPN))
            {
                var existingPupil = _schoolService.GetMatchedPupil(RouteData.Values["phase"].ToString(), addPupilViewModel.UPN);
                if (existingPupil == null)
                {
                    ModelState.AddModelError("UPN", "Enter a valid UPN");
                }
                else
                {
                    var addPupilAmendment = new AddPupilAmendmentViewModel
                    {
                        URN = ClaimsHelper.GetURN(this.User),
                        PupilViewModel = existingPupil.PupilViewModel,
                        Results = existingPupil.Results
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
                    PupilViewModel = addPupilViewModel,
                    Results = new List<PriorAttainmentResultViewModel>()
                };
                HttpContext.Session.Set(ADD_PUPIL_AMENDMENT, addPupilAmendment);
                return RedirectToAction("Add", "PriorAttainment");
            }

            return View(addPupilViewModel);
        }

        public IActionResult ExistingMatch()
        {
            var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>(ADD_PUPIL_AMENDMENT);
            if (addPupilAmendment.PupilViewModel == null)
            {
                return RedirectToAction("Add");
            }

            var school = _schoolService.GetSchoolName(addPupilAmendment.PupilViewModel.SchoolID);
            return View(new ExistingMatchViewModel{ PupilViewModel = addPupilAmendment.PupilViewModel, SchoolName = school});
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
                return View(new UploadEvidenceViewModel{ PupilViewModel = addPupilAmendment.PupilViewModel });
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
            
            return View(new ConfirmAddPupilViewModel{ PupilViewModel = addPupilAmendment.PupilViewModel, SelectedEvidenceOption = addPupilAmendment.SelectedEvidenceOption});
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
    }
}
