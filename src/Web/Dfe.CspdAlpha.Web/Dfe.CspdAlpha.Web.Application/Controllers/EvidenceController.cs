using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Evidence;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Dfe.Rscd.Web.ApiClient;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class EvidenceController : SessionController
    {
        private readonly IAmendmentService _amendmentService;
        private readonly IEvidenceService _evidenceService;

        public EvidenceController(IEvidenceService evidenceService, IAmendmentService amendmentService)
        {
            _amendmentService = amendmentService;
            _evidenceService = evidenceService;
        }

        public IActionResult Index()
        {
            var amendment = GetAmendment();
            var amendmentDetail = amendment.AmendmentDetail;

            return View(new EvidenceViewModel
            {
                PupilDetails = new PupilViewModel(amendment.Pupil),
                AddReason = amendmentDetail.GetField<string>("AddReason")
            });
        }

        [HttpPost]
        public IActionResult Index(EvidenceViewModel viewModel)
        {
            var amendment = GetAmendment();
            amendment.EvidenceStatus = viewModel.EvidenceOption;
            SaveAmendment(amendment);

            if (ModelState.IsValid)
                switch (viewModel.EvidenceOption)
                {
                    case EvidenceStatus.Now:
                        return RedirectToAction("Upload");
                    case EvidenceStatus.Later:
                    case EvidenceStatus.NotRequired:
                        return RedirectToAction("Prompt", "Amendments");
                    default:
                        var amendmentDetail = amendment.AmendmentDetail;
                        viewModel.PupilDetails = new PupilViewModel(amendment.Pupil);
                        viewModel.AddReason = amendmentDetail.GetField<string>(Constants.AddPupil.AddReason);
                        return View(viewModel);
                }

            return View(viewModel);
        }

        public IActionResult Upload(string id)
        {
            // Upload for existing amendment
            if (id == null)
            {
                var amendment = GetAmendment();
                if (amendment == null || amendment.EvidenceStatus != EvidenceStatus.Now)
                    return RedirectToAction("Index", "AddPupil");
                return View(new UploadViewModel
                {
                    Pupil = new PupilViewModel(amendment.Pupil), AmendmentType = amendment.AmendmentType
                });
            }
            //Upload for amendment in progress
            else
            {
                var amendment = _amendmentService.GetAmendment(id);

                return View(new UploadViewModel {Pupil = new PupilViewModel(amendment.Pupil)});
            }
        }

        [HttpPost]
        public IActionResult Upload(UploadViewModel viewModel, string id)
        {
            if (id == null)
            {
                var amendment = GetAmendment();
                if (ModelState.IsValid)
                {
                    amendment.EvidenceFolderName = _evidenceService.UploadEvidence(viewModel.EvidenceFiles);
                    if (!string.IsNullOrWhiteSpace(amendment.EvidenceFolderName))
                    {
                        SaveAmendment(amendment);
                        return RedirectToAction("Prompt", "Amendments");
                    }

                    ModelState.AddModelError("EvidenceFiles", "Upload file");
                }

                viewModel.Pupil = new PupilViewModel(amendment.Pupil);
                viewModel.AmendmentType = amendment.AmendmentType;
                return View(viewModel);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var uploadedEvidenceFiles = _evidenceService.UploadEvidence(viewModel.EvidenceFiles);
                    _amendmentService.RelateEvidence(id, uploadedEvidenceFiles);
                    return RedirectToAction("Index", "Amendments");
                }

                var amendment = _amendmentService.GetAmendment(id);

                return View(new UploadViewModel {Pupil = new PupilViewModel(amendment.Pupil)});
            }
        }
    }
}
