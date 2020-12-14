using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Dfe.CspdAlpha.Web.Application.Application;
using Dfe.Rscd.Web.ApiClient;
using Microsoft.Extensions.Configuration;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class RemovePupilController : Controller
    {
        private IPupilService _pupilService;
        private IAmendmentService _amendmentService;
        private IConfiguration _config;
        private CheckingWindow CheckingWindow => CheckingWindowHelper.GetCheckingWindow(RouteData);

        public RemovePupilController(IPupilService pupilService, IAmendmentService amendmentService, IConfiguration config)
        {
            _config = config;
            _amendmentService = amendmentService;
            _pupilService = pupilService;
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
            var pupilsFound = _pupilService.GetPupilDetailsList(CheckingWindow, viewModel);
            if (pupilsFound.Count == 0 || pupilsFound.Count > 1)
            {
                return View(new ResultsViewModel
                {
                    PupilList = pupilsFound,
                    SearchType = viewModel.SearchType,
                    Query = viewModel.Query
                });
            }
            return RedirectToAction("MatchedPupil", new { id = pupilsFound.First().Id });
        }

        [HttpPost]
        public IActionResult Results(ResultsViewModel viewModel, string urn)
        {
            if (ModelState.IsValid)
            {
                SavePupilToSession(viewModel.SelectedID, urn);
                return RedirectToAction("Reason", new { searchType= viewModel.SearchType, query = viewModel.Query });
            }
            viewModel.PupilList = _pupilService.GetPupilDetailsList(CheckingWindow, new SearchQuery { Query = viewModel.Query, URN = viewModel.URN, SearchType = viewModel.SearchType});

            return View(viewModel);
        }

        private RemovePupilViewModel SavePupilToSession(string id, string urn)
        {
            var viewModel = _pupilService.GetPupil(CheckingWindow, id);
            var amendment = new Amendment
            {
                Urn = urn,
                CheckingWindow = CheckingWindowHelper.GetCheckingWindow(RouteData),
                AmendmentType = AmendmentType.RemovePupil,
                Pupil = new PupilDetails
                {
                    KeyStage = viewModel.PupilViewModel.Keystage,
                    Id = id,
                    Upn = viewModel.PupilViewModel.UPN,
                    Uln = viewModel.PupilViewModel.ULN,
                    ForeName = viewModel.PupilViewModel.FirstName,
                    LastName = viewModel.PupilViewModel.LastName,
                    DateOfBirth = viewModel.PupilViewModel.DateOfBirth,
                    Age = viewModel.PupilViewModel.Age,
                    Gender = viewModel.PupilViewModel.Gender,
                    DateOfAdmission = viewModel.PupilViewModel.DateOfAdmission,
                    YearGroup = viewModel.PupilViewModel.YearGroup
                },
                AmendmentDetail = new AmendmentDetail()
            };
            HttpContext.Session.Set(Constants.AMENDMENT_SESSION_KEY, amendment);
            return new RemovePupilViewModel {PupilViewModel = viewModel.PupilViewModel};
        }

        public IActionResult MatchedPupil(QueryType searchType, string query, string id, string urn)
        {
            var viewModel = SavePupilToSession(id, urn);
            viewModel.QueryType = searchType;
            viewModel.query = query;
            return View(viewModel);
        }

        public IActionResult Reason(QueryType searchType, string query, string matchedId)
        {
            var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);

            if (amendment == null)
            {
                return RedirectToAction("Index");
            }

            var amendmentDetail = amendment.AmendmentDetail;

            return View(new ReasonViewModel
            {
                PupilDetails = (PupilDetails)amendment.Pupil,
                SelectedReasonCode = amendmentDetail.GetField<int?>("ReasonCode"),
                SearchType = searchType,
                Query = query,
                MatchedId = matchedId
            });
        }

        [HttpPost]
        public IActionResult Reason(ReasonViewModel viewModel)
        {
            var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);

            if (ModelState.IsValid)
            {
                amendment.AmendmentDetail.AddField("ReasonCode", viewModel.SelectedReasonCode.Value);
                amendment.EvidenceStatus = viewModel.SelectedReasonCode == 329 ? EvidenceStatus.Now : EvidenceStatus.NotRequired;
                HttpContext.Session.Set(Constants.AMENDMENT_SESSION_KEY, amendment);
                if (new[]
                    {
                        Constants.NOT_AT_END_OF_16_TO_18_STUDY,
                        Constants.INTERNATIONAL_STUDENT,
                        Constants.DECEASED,
                        Constants.NOT_ON_ROLL
                    }.Any(r => r == viewModel.SelectedReasonCode))
                {
                    return RedirectToAction("Confirm", "Amendments");
                }
                if (new[]
                    {
                        Constants.OTHER_WITH_EVIDENCE,
                        Constants.OTHER_EVIDENCE_NOT_REQUIRED
                    }
                    .Any(r => r == viewModel.SelectedReasonCode))
                {
                    return RedirectToAction("Details");
                }
            }
            return View(new ReasonViewModel { PupilDetails = (PupilDetails) amendment.Pupil });
        }

        public IActionResult Details()
        {
            var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            if (amendment == null)
            {
                return RedirectToAction("Index");
            }

            var amendmentDetail = amendment.AmendmentDetail;
            return View(new DetailsViewModel
            {
                PupilDetails = (PupilDetails)amendment.Pupil,
                AmendmentDetails = amendmentDetail.GetField<string>("Detail")
            });
        }

        [HttpPost]
        public IActionResult Details(DetailsViewModel viewModel)
        {
            var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            var amendmentDetail = amendment.AmendmentDetail;

            if (ModelState.IsValid)
            {
                amendmentDetail.AddField("Detail", viewModel.AmendmentDetails);

                HttpContext.Session.Set(Constants.AMENDMENT_SESSION_KEY, amendment);

                if (amendment.EvidenceStatus == EvidenceStatus.Now)
                {
                    return RedirectToAction("Upload", "Evidence");
                }

                return RedirectToAction("Confirm","Amendments");
            }

            viewModel.PupilDetails = (PupilDetails)amendment.Pupil;
            return View(viewModel);
        }
    }
}
