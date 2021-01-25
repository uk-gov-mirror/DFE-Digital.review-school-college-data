using Dfe.CspdAlpha.Web.Application.Application;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Microsoft.AspNetCore.Mvc;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil;
using Dfe.Rscd.Web.ApiClient;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    [TasksReviewedFilter("Index,View")]
    public class PupilController : SessionController
    {
        private IEstablishmentService _establishmentService;
        private IPupilService _pupilService;
        private CheckingWindow CheckingWindow => CheckingWindowHelper.GetCheckingWindow(RouteData);

        public PupilController(IPupilService pupilService, IEstablishmentService establishmentService)
        {
            _pupilService = pupilService;
            _establishmentService = establishmentService;
        }

        public IActionResult Index(string urn)
        {
            var pupilList = _pupilService.GetPupilDetailsList(CheckingWindow, new SearchQuery { URN = urn });
            var viewModel = new PupilListViewModel
            {
                CheckingWindow = CheckingWindow,
                Pupils = pupilList,
                SchoolDetails = _establishmentService.GetSchoolDetails(CheckingWindow, urn)
            };
            return View(viewModel);
        }

        public new IActionResult View(string id)
        {
            var viewModel = _pupilService.GetPupil(CheckingWindow, id);
            return View(new ViewPupilViewModel{CheckingWindow = CheckingWindow, MatchedPupilViewModel = viewModel});
        }

        public IActionResult CancelAmendment()
        {
            ClearAmendmentAndRelated();
            return RedirectToAction("Index");
        }
    }
}
