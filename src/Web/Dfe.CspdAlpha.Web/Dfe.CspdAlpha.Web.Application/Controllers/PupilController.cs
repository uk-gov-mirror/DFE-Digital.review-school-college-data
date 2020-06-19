using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Application.Application.Extensions;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Dfe.CspdAlpha.Web.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class PupilController : Controller
    {
        private ISchoolService _schoolService;
        private readonly IAmendmentService _amendmentService;

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

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddPupilViewModel addPupilViewModel)
        {
            if (ModelState.IsValid)
            {
                var addPupilAmendment = new AddPupilAmendmentViewModel{AddPupilViewModel = addPupilViewModel};
                HttpContext.Session.Set("add-pupil-amendment", addPupilAmendment);
                return RedirectToAction("AddResult");
            }
            return View(addPupilViewModel);
        }

        public IActionResult AddResult()
        {
            var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>("add-pupil-amendment");
            return View(addPupilAmendment);
        }

        [HttpPost]
        public IActionResult AddResult(AddPriorAttainmentViewModel addPriorAttainmentViewModel)
        {
            var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>("add-pupil-amendment");
            addPupilAmendment.AddPriorAttainmentViewModel = addPriorAttainmentViewModel;
            if (ModelState.IsValid)
            {
                HttpContext.Session.Set("add-pupil-amendment", addPupilAmendment);
                return RedirectToAction("AddEvidence");
            }
            return View(addPupilAmendment);
        }

        public IActionResult AddEvidence()
        {
            var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>("add-pupil-amendment");
            return View(addPupilAmendment);
        }

        [HttpPost]
        public IActionResult AddEvidence(EvidenceOption selectedEvidenceOption)
        {
            var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>("add-pupil-amendment");
            addPupilAmendment.SelectedEvidenceOption = selectedEvidenceOption;
            if (ModelState.IsValid)
            {
                HttpContext.Session.Set("add-pupil-amendment", addPupilAmendment);
                switch (selectedEvidenceOption)
                {
                    case EvidenceOption.UploadNow:
                        return RedirectToAction("UploadEvidence");
                    case EvidenceOption.UploadLater:
                    case EvidenceOption.NotRequired:
                        return RedirectToAction("InclusionDetails");
                    default:
                        return View(addPupilAmendment);
                }
            }
            return View(addPupilAmendment);
        }

        public IActionResult UploadEvidence()
        {
            var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>("add-pupil-amendment");
            return View(addPupilAmendment);
        }

        [HttpPost]
        public IActionResult UploadEvidence(List<string> evidenceFiles)
        {
            var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>("add-pupil-amendment");
            return View(addPupilAmendment);
        }

        public IActionResult CancelAmendment()
        {
            HttpContext.Session.Remove("add-pupil-amendment");
            return RedirectToAction("Index");
        }

        public IActionResult DynamicsReadTest()
        {
            var result = _amendmentService.GetAddPupilAmendments(7654321);

            return Json(result);
        }

        public IActionResult DynamicsCreateTest()
        {
            var result = _amendmentService.CreateAddPupilAmendment(null);

            return Json(result);
        }
    }
}
