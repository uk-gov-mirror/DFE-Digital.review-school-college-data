using Dfe.CspdAlpha.Web.Application.Application.Extensions;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
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
                HttpContext.Session.Set("add-pupil-amendment", addPupilViewModel);
                return RedirectToAction("AddResult");
            }
            return View(addPupilViewModel);
        }

        public IActionResult AddResult()
        {
            var addPupilViewModel = HttpContext.Session.Get<AddPupilViewModel>("add-pupil-amendment");
            return View(addPupilViewModel);
        }
    }
}
