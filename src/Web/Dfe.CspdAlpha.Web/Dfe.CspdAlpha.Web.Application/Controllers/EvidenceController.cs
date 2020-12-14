using System;
using Dfe.CspdAlpha.Web.Application.Application;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Evidence;
using Dfe.Rscd.Web.ApiClient;
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
            var amendmentDetail = amendment?.AmendmentDetail;
            if (amendment.Pupil == null)
            {
                return RedirectToAction("Index", "AddPupil");
            }
            return View(new EvidenceViewModel{PupilDetails = (PupilDetails)amendment.Pupil, AddReason = amendmentDetail.GetField<string>("AddReason")});
        }

        [HttpPost]
        public IActionResult Index(EvidenceViewModel viewModel)
        {
            var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            amendment.EvidenceStatus = viewModel.EvidenceOption;
            HttpContext.Session.Set(Constants.AMENDMENT_SESSION_KEY, amendment);
            if (ModelState.IsValid)
            {
                switch (viewModel.EvidenceOption)
                {
                    case EvidenceStatus.Now:
                        return RedirectToAction("Upload");
                    case EvidenceStatus.Later:
                    case EvidenceStatus.NotRequired:
                        return RedirectToAction("Confirm", "Amendments");
                    default:
                        var amendmentDetail = amendment?.AmendmentDetail;
                        viewModel.PupilDetails = (PupilDetails)amendment.Pupil;
                        viewModel.AddReason = amendmentDetail.GetField<string>("AddReason");
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

                if (amendment == null || amendment.EvidenceStatus != EvidenceStatus.Now)
                {
                    return RedirectToAction("Index", "AddPupil");
                }
                return View(new UploadViewModel { PupilDetails = (PupilDetails)amendment.Pupil, AmendmentType = amendment.AmendmentType});
            }
            //Upload for amendment in progress
            else
            {
                var amendment = _amendmentService.GetAmendment(CheckingWindow, id);

                return View(new UploadViewModel { PupilDetails = (PupilDetails)amendment.Pupil });
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
                    if (!string.IsNullOrWhiteSpace(amendment.EvidenceFolderName))
                    {
                        HttpContext.Session.Set(Constants.AMENDMENT_SESSION_KEY, amendment);
                        return RedirectToAction("Confirm", "Amendments");
                    }
                    ModelState.AddModelError("EvidenceFiles", "Upload file");
                }

                viewModel.PupilDetails = (PupilDetails)amendment.Pupil;
                viewModel.AmendmentType = amendment.AmendmentType;
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
                var amendment = _amendmentService.GetAmendment(CheckingWindow, id);

                return View(new UploadViewModel { PupilDetails = (PupilDetails)amendment.Pupil });
            }
        }
    }
}
