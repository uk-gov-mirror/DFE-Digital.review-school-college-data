using System;
using Dfe.CspdAlpha.Web.Application.Application.Extensions;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.Amendments;
using Dfe.CspdAlpha.Web.Application.Models.Amendments.AmendmentTypes;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Evidence;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class EvidenceController : Controller
    {
        private IEvidenceService _evidenceService;
        private IAmendmentService _amendmentService;
        private CheckingWindow CheckingWindow => CheckingWindowHelper.GetCheckingWindow(RouteData);

        public EvidenceController(IEvidenceService evidenceService, IAmendmentService amendmentService)
        {
            _amendmentService = amendmentService;
            _evidenceService = evidenceService;
        }

        public IActionResult Index()
        {
            var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            var amendmentDetail = (AddPupil)amendment?.AmendmentDetail;
            if (amendment.PupilDetails == null)
            {
                return RedirectToAction("Index", "AddPupil");
            }
            return View(new EvidenceViewModel{PupilDetails = amendment.PupilDetails, AddReason = amendmentDetail.AddReason});
        }

        [HttpPost]
        public IActionResult Index(EvidenceViewModel viewModel)
        {
            var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            amendment.EvidenceOption = viewModel.EvidenceOption;
            HttpContext.Session.Set(Constants.AMENDMENT_SESSION_KEY, amendment);
            if (ModelState.IsValid)
            {
                switch (viewModel.EvidenceOption)
                {
                    case EvidenceOption.UploadNow:
                        return RedirectToAction("Upload");
                    case EvidenceOption.UploadLater:
                    case EvidenceOption.NotRequired:
                        return RedirectToAction("Confirm", "Amendments");
                    default:
                        var amendmentDetail = (AddPupil)amendment?.AmendmentDetail;
                        viewModel.PupilDetails = amendment.PupilDetails;
                        viewModel.AddReason = amendmentDetail.AddReason;
                        return View(viewModel);
                }
            }
            return View(viewModel);
        }

        public IActionResult Upload(string id)
        {
            // Upload for existing amendment
            if (id == null)
            {
                var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);

                if (amendment == null || amendment.EvidenceOption != EvidenceOption.UploadNow)
                {
                    return RedirectToAction("Index", "AddPupil");
                }
                return View(new UploadViewModel { PupilDetails = amendment.PupilDetails, AmendmentType = amendment.AmendmentDetail.AmendmentType });
            }
            //Upload for amendment in progress
            else
            {
                var amendment = _amendmentService.GetAmendment(CheckingWindow, new Guid(id));
                var pupilDetails = new PupilDetails
                {
                    UPN = amendment.PupilViewModel.UPN,
                    FirstName = amendment.PupilViewModel.FirstName,
                    LastName = amendment.PupilViewModel.LastName,
                    DateOfBirth = amendment.PupilViewModel.DateOfBirth,
                    Gender = amendment.PupilViewModel.Gender,
                    Age = amendment.PupilViewModel.Age,
                    DateOfAdmission = amendment.PupilViewModel.DateOfAdmission,
                    YearGroup = amendment.PupilViewModel.YearGroup,
                    Keystage = amendment.PupilViewModel.Keystage
                };

                return View(new UploadViewModel { PupilDetails = pupilDetails});
            }
        }

        [HttpPost]
        public IActionResult Upload(UploadViewModel viewModel, string id)
        {
            if (id == null)
            {
                var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);

                if (ModelState.IsValid)
                {
                    amendment.EvidenceFolderName = _evidenceService.UploadEvidence(viewModel.EvidenceFiles);
                    HttpContext.Session.Set(Constants.AMENDMENT_SESSION_KEY, amendment);
                    return RedirectToAction("Confirm", "Amendments");
                }

                viewModel.PupilDetails = amendment.PupilDetails;
                viewModel.AmendmentType = amendment.AmendmentDetail.AmendmentType;
                return View(viewModel);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var uploadedEvidenceFiles = _evidenceService.UploadEvidence(viewModel.EvidenceFiles);
                    _amendmentService.RelateEvidence(CheckingWindow, id, uploadedEvidenceFiles);
                    return RedirectToAction("Index", "Amendments");
                }
                var amendment = _amendmentService.GetAmendment(CheckingWindow, new Guid(id));
                var pupilDetails = new PupilDetails
                {
                    UPN = amendment.PupilViewModel.UPN,
                    FirstName = amendment.PupilViewModel.FirstName,
                    LastName = amendment.PupilViewModel.LastName,
                    DateOfBirth = amendment.PupilViewModel.DateOfBirth,
                    Gender = amendment.PupilViewModel.Gender,
                    Age = amendment.PupilViewModel.Age,
                    DateOfAdmission = amendment.PupilViewModel.DateOfAdmission,
                    YearGroup = amendment.PupilViewModel.YearGroup,
                    Keystage = amendment.PupilViewModel.Keystage
                };

                return View(new UploadViewModel { PupilDetails = pupilDetails });
            }
        }
    }
}
