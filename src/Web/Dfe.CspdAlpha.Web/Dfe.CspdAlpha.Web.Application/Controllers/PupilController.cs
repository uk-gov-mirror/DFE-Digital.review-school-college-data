using Dfe.Rscd.Web.Application.Application;
using Dfe.Rscd.Web.Application.Application.Interfaces;
using Dfe.Rscd.Web.Application.Models.ViewModels.Pupil;
using Dfe.Rscd.Web.Application.Models.ViewModels.RemovePupil;
using Dfe.Rscd.Web.Application.Security;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Rscd.Web.Application.Controllers
{
    [TasksReviewedFilter("Index,View")]
    public class PupilController : SessionController
    {
        private readonly IEstablishmentService _establishmentService;
        private readonly IPupilService _pupilService;

        public PupilController(IPupilService pupilService, IEstablishmentService establishmentService, UserInfo userInfo)
            : base(userInfo)
        {
            _pupilService = pupilService;
            _establishmentService = establishmentService;
        }

        public IActionResult Index(string urn)
        {
            var pupilList = _pupilService.GetPupilDetailsList(new SearchQuery {URN = urn});
            var viewModel = new PupilListViewModel
            {
                Pupils = pupilList,
                SchoolDetails = _establishmentService.GetSchoolDetails(urn)
            };
            return View(viewModel);
        }

        public new IActionResult View(string id)
        {
            var viewModel = _pupilService.GetPupil(id);
            return View(new ViewPupilViewModel { MatchedPupilViewModel = viewModel });
        }

        public IActionResult CancelAmendment()
        {
            ClearAmendmentAndRelated();
            return RedirectToAction("Index");
        }
    }
}
