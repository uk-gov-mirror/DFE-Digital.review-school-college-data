using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class RemovePupilController : Controller
    {
        private ISchoolService _schoolService;
        private CheckingWindow CheckingWindow => CheckingWindowHelper.GetCheckingWindow(RouteData);

        public RemovePupilController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(SearchPupilsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Results", new { viewModel.SearchType, Query = viewModel.PupilID ?? viewModel.Name });
            }
            return View(viewModel);
        }

        public IActionResult Results(SearchQuery viewModel)
        {
            var pupilsFound = _schoolService.GetPupilListViewModel(CheckingWindow, viewModel);
            if (pupilsFound.Pupils.Count == 0 || pupilsFound.Pupils.Count > 1)
            {
                return View( new ResultsViewModel
                {
                    PupilListViewModel = pupilsFound
                });
            }
            return RedirectToAction("MatchedPupil", new { id = pupilsFound.Pupils.First().PupilId });
        }

        [HttpPost]
        public IActionResult Results(ResultsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("MatchedPupil", new { id = viewModel.SelectedID });
            }
            viewModel.PupilListViewModel = _schoolService.GetPupilListViewModel(CheckingWindow, new SearchQuery { Query = viewModel.Query, URN = viewModel.URN, SearchType = viewModel.SearchType});

            return View(viewModel);

        }

        public IActionResult MatchedPupil(string id)
        {
            var viewModel = _schoolService.GetPupil(CheckingWindow, id);
            return View(viewModel);
        }
    }
}
