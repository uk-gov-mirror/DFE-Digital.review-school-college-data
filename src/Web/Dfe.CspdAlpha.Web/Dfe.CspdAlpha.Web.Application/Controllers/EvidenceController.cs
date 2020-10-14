using Dfe.CspdAlpha.Web.Application.Application.Extensions;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.Amendments;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Evidence;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class EvidenceController : Controller
    {
        private IAmendmentService _amendmentService;

        public EvidenceController(IAmendmentService amendmentService)
        {
            _amendmentService = amendmentService;
        }

        public IActionResult Upload(string id)
        {
            // Upload for existing amendment
            //if (id == null)
            //{
                var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);

                //if (amendment == null || amendment.EvidenceOption != Models.Common.EvidenceOption.UploadNow)
                //{
                //    return RedirectToAction("Add");
                //}
                return View(new UploadViewModel {ID = id, PupilDetails= amendment.PupilDetails, AmendmentType = amendment.AmendmentDetail.AmendmentType});
            //}
            // Upload for amendment in progress
            //else
            //{
            //    var addPupilAmendment = _amendmentService.GetAddPupilAmendmentViewModel(new Guid(id));
            //    return View(new UploadEvidenceViewModel { PupilViewModel = addPupilAmendment.PupilViewModel, Id = id });
            //}
        }

        [HttpPost]
        public IActionResult Upload(UploadViewModel viewModel)
        {
            //if (id == null)
            //{
            var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);

            if (ModelState.IsValid)
            {
                amendment.EvidenceFolderName = _amendmentService.UploadEvidence(viewModel.EvidenceFiles);
                HttpContext.Session.Set(Constants.AMENDMENT_SESSION_KEY, amendment);
                return RedirectToAction("Confirm", "Amendments");
            }

            viewModel.PupilDetails = amendment.PupilDetails;
            viewModel.AmendmentType = amendment.AmendmentDetail.AmendmentType;
            return View(viewModel);
            //}
            //else
            //{
            //    if (ModelState.IsValid)
            //    {
            //        var uploadedEvidenceFiles = _amendmentService.UploadEvidence(evidenceFiles);
            //        _amendmentService.RelateEvidence(new Guid(id), uploadedEvidenceFiles);
            //        return RedirectToAction("Index", "Amendments");
            //    }
            //    var addPupilAmendment = _amendmentService.GetAddPupilAmendmentViewModel(new Guid(id));
            //    return View(new UploadEvidenceViewModel { PupilViewModel = addPupilAmendment.PupilViewModel, Id = id });
            //}
        }
    }
}
