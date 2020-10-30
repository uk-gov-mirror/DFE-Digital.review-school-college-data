using Dfe.CspdAlpha.Web.Application.Application.Extensions;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.Amendments;
using Dfe.CspdAlpha.Web.Application.Models.Amendments.AmendmentTypes;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
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
            return RedirectToAction("MatchedPupil", new { id = pupilsFound.First().ID });
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
                URN = urn,
                CheckingWindow = CheckingWindowHelper.GetCheckingWindow(RouteData),
                PupilDetails = new PupilDetails
                {
                    Keystage = viewModel.PupilViewModel.Keystage,
                    ID = id,
                    UPN = viewModel.PupilViewModel.UPN,
                    ULN = viewModel.PupilViewModel.ULN,
                    FirstName = viewModel.PupilViewModel.FirstName,
                    LastName = viewModel.PupilViewModel.LastName,
                    DateOfBirth = viewModel.PupilViewModel.DateOfBirth,
                    Age = viewModel.PupilViewModel.Age,
                    Gender = viewModel.PupilViewModel.Gender,
                    DateOfAdmission = viewModel.PupilViewModel.DateOfAdmission,
                    YearGroup = viewModel.PupilViewModel.YearGroup
                },
                AmendmentDetail = new RemovePupil()
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

            var removePupil = (RemovePupil)amendment.AmendmentDetail;
            return View(new ReasonViewModel
            {
                PupilDetails = amendment.PupilDetails,
                Reasons = _amendmentService.GetRemoveReasons(),
                SelectedReason = removePupil.Reason,
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
                var removePupil = (RemovePupil)amendment.AmendmentDetail;
                removePupil.Reason = viewModel.SelectedReason;
                amendment.EvidenceOption = viewModel.SelectedReason == "329" ? EvidenceOption.UploadNow : EvidenceOption.NotRequired;
                HttpContext.Session.Set(Constants.AMENDMENT_SESSION_KEY, amendment);
                if (new[]
                    {
                        Constants.NOT_AT_END_OF_16_TO_18_STUDY,
                        Constants.INTERNATIONAL_STUDENT,
                        Constants.DECEASED
                    }.Any(r => r == viewModel.SelectedReason))
                {
                    return RedirectToAction("Confirm", "Amendments");
                }
                if (new[] {Constants.OTHER_WITH_EVIDENCE}.Any(r => r == viewModel.SelectedReason))
                { 
                    return RedirectToAction("SubReason");
                }
                if (new[] { Constants.NOT_ON_ROLL }.Any(r => r == viewModel.SelectedReason))
                {
                    return RedirectToAction("AllocationYear");
                }
                if (new[] { Constants.OTHER_EVIDENCE_NOT_REQUIRED }.Any(r => r == viewModel.SelectedReason))
                {
                    return RedirectToAction("Details");
                }
            }
            return View(new ReasonViewModel { PupilDetails = amendment.PupilDetails, Reasons = _amendmentService.GetRemoveReasons() });
        }

        public IActionResult SubReason()
        {
            var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            if (amendment == null)
            {
                return RedirectToAction("Index");
            }

            var removePupil = (RemovePupil)amendment.AmendmentDetail;
            return View(new SubReasonViewModel
            {
                PupilDetails = amendment.PupilDetails,
                Reasons = _amendmentService.GetRemoveReasons(removePupil.Reason),
                SelectedReason = removePupil.SubReason
            });
        }

        [HttpPost]
        public IActionResult SubReason(SubReasonViewModel viewModel)
        {
            var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            var removePupil = (RemovePupil)amendment.AmendmentDetail;
            if (ModelState.IsValid)
            {
                removePupil.SubReason = viewModel.SelectedReason;
                HttpContext.Session.Set(Constants.AMENDMENT_SESSION_KEY, amendment);
                return RedirectToAction("Details");
            }
            return View(new SubReasonViewModel { PupilDetails = amendment.PupilDetails, Reasons = _amendmentService.GetRemoveReasons(removePupil.Reason) });
        }

        public IActionResult Details()
        {
            var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            if (amendment == null)
            {
                return RedirectToAction("Index");
            }
            var removePupil = (RemovePupil)amendment.AmendmentDetail;
            return View(new DetailsViewModel
            {
                PupilDetails = amendment.PupilDetails,
                Reason = removePupil.Reason,
                AmendmentDetails = removePupil.Detail
            });
        }

        [HttpPost]
        public IActionResult Details(DetailsViewModel viewModel)
        {
            var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            var removePupil = (RemovePupil)amendment.AmendmentDetail;
            if (ModelState.IsValid)
            {
                removePupil.Detail = viewModel.AmendmentDetails;
                HttpContext.Session.Set(Constants.AMENDMENT_SESSION_KEY, amendment);
                if (removePupil.Reason == Constants.OTHER_EVIDENCE_NOT_REQUIRED)
                {
                    return RedirectToAction("AllocationYear");
                }
                if (amendment.EvidenceOption == EvidenceOption.UploadNow)
                {
                    return RedirectToAction("Upload", "Evidence");
                }
                return RedirectToAction("Confirm","Amendments");
            }

            viewModel.PupilDetails = amendment.PupilDetails;
            viewModel.Reason = removePupil.Reason;
            return View(viewModel);
        }

        public IActionResult AllocationYear()
        {
            var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            if (amendment == null)
            {
                return RedirectToAction("Index");
            }
            var removePupil = (RemovePupil)amendment.AmendmentDetail;
            return View(new AllocationYearViewModel(_config["AllocationYear"])
            {
                PupilDetails = amendment.PupilDetails,
                Reason = removePupil.Reason,
                AllocationYear = removePupil.AllocationYear
            });
        }

        [HttpPost]
        public IActionResult AllocationYear(AllocationYearViewModel viewModel)
        {
            var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            var removePupil = (RemovePupil)amendment.AmendmentDetail;
            if (ModelState.IsValid)
            {
                removePupil.AllocationYear = viewModel.AllocationYear;
                HttpContext.Session.Set(Constants.AMENDMENT_SESSION_KEY, amendment);
                return RedirectToAction("Confirm", "Amendments");
            }

            return View(new AllocationYearViewModel(_config["AllocationYear"])
            {
                PupilDetails = amendment.PupilDetails,
                Reason = removePupil.Reason,
                AllocationYear = viewModel.AllocationYear
            });
        }

    }
}
