using Dfe.CspdAlpha.Web.Application.Application.Extensions;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class PupilController : Controller
    {
        private ISchoolService _schoolService;

        public PupilController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
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

        public IActionResult CancelAmendment()
        {
            HttpContext.Session.Remove("add-pupil-amendment");
            return RedirectToAction("Index");
        }
    }
}
