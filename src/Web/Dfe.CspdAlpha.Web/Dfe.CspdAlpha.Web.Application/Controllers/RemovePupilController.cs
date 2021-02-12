using System.Linq;
using Dfe.Rscd.Web.ApiClient;
using Dfe.Rscd.Web.Application.Application.Helpers;
using Dfe.Rscd.Web.Application.Application.Interfaces;
using Dfe.Rscd.Web.Application.Models.Common;
using Dfe.Rscd.Web.Application.Models.ViewModels.Pupil;
using Dfe.Rscd.Web.Application.Models.ViewModels.RemovePupil;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Rscd.Web.Application.Controllers
{
    public class RemovePupilController : SessionController
    {
        private readonly IPupilService _pupilService;

        public RemovePupilController(IPupilService pupilService)
        {
            _pupilService = pupilService;
        }

        public IActionResult Index()
        {
            return View(new SearchPupilsViewModel{ Name = string.Empty, PupilID = string.Empty });
        }

        [HttpPost]
        public IActionResult Index(SearchPupilsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Results", new { viewModel.SearchType, Query = viewModel.PupilID ?? viewModel.Name, CheckingWindow });
            }
            return View(viewModel);
        }

        public IActionResult Results(SearchQuery viewModel)
        {
            var pupilsFound = _pupilService.GetPupilDetailsList(viewModel);
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
            viewModel.PupilList = _pupilService.GetPupilDetailsList(new SearchQuery { Query = viewModel.Query, URN = viewModel.URN, SearchType = viewModel.SearchType});

            return View(viewModel);
        }

        private RemovePupilViewModel SavePupilToSession(string id, string urn)
        {
            var viewModel = _pupilService.GetPupil(id);

            var amendment = new Amendment
            {
                Urn = urn,
                CheckingWindow = CheckingWindowHelper.GetCheckingWindow(RouteData),
                AmendmentType = AmendmentType.RemovePupil,
                Pupil = new Pupil
                {
                    Id = id,
                    Upn = viewModel.PupilViewModel.UPN,
                    Uln = viewModel.PupilViewModel.ULN,
                    Forename = viewModel.PupilViewModel.FirstName,
                    Surname = viewModel.PupilViewModel.LastName,
                    Dob = viewModel.PupilViewModel.DateOfBirth,
                    Age = viewModel.PupilViewModel.Age,
                    Gender = viewModel.PupilViewModel.Gender,
                    AdmissionDate = viewModel.PupilViewModel.DateOfAdmission,
                    YearGroup = viewModel.PupilViewModel.YearGroup,
                    Allocations = viewModel.PupilViewModel.Allocations,
                    Pincl = new PInclude{ Code = viewModel.PupilViewModel.PincludeCode }
                },
                AmendmentDetail = new AmendmentDetail(),
                IsUserConfirmed = false
            };

            SaveAmendment(amendment);

            return new RemovePupilViewModel { MatchedPupilViewModel = viewModel };
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
            var amendment = GetAmendment();
            ClearQuestions();

            if (amendment == null)
            {
                return RedirectToAction("Index");
            }

            var amendmentDetail = amendment.AmendmentDetail;
            var reasons = _pupilService.GetAmendmentReasons(amendment.Pupil.Pincl.Code);

            return View(new ReasonViewModel
            {
                PupilDetails = new PupilViewModel(amendment.Pupil),
                SelectedReasonCode = amendmentDetail.GetField<int?>(Constants.RemovePupil.FIELD_ReasonCode),
                SearchType = searchType,
                Query = query,
                MatchedId = matchedId,
                Reasons = reasons.ToList()
            });
        }

        [HttpPost]
        public IActionResult Reason(ReasonViewModel viewModel)
        {
            var amendment = GetAmendment();

            if (ModelState.IsValid)
            {
                amendment.AmendmentDetail.AddField(Constants.RemovePupil.FIELD_ReasonCode, viewModel.SelectedReasonCode.Value);
                amendment.InclusionReasonId = viewModel.SelectedReasonCode.Value;

                amendment.EvidenceStatus = EvidenceStatus.NotRequired;

                SaveAmendment(amendment);

                return RedirectToAction("Prompt", "Amendments");
            }
            return View(new ReasonViewModel { PupilDetails = new PupilViewModel(amendment.Pupil) });
        }

        public IActionResult Details()
        {
            var amendment = GetAmendment();
            if (amendment == null)
            {
                return RedirectToAction("Index");
            }

            var amendmentDetail = amendment.AmendmentDetail;
            return View(new DetailsViewModel
            {
                PupilDetails = new PupilViewModel(amendment.Pupil),
                AmendmentDetails = amendmentDetail.GetField<string>(Constants.RemovePupil.FIELD_UserProvidedDetails)
            });
        }

        [HttpPost]
        public IActionResult Details(DetailsViewModel viewModel)
        {
            var amendment = GetAmendment();
            var amendmentDetail = amendment.AmendmentDetail;

            if (ModelState.IsValid)
            {
                amendmentDetail.AddField(Constants.RemovePupil.FIELD_UserProvidedDetails, viewModel.AmendmentDetails);

                SaveAmendment(amendment);

                if (amendment.EvidenceStatus == EvidenceStatus.Now)
                {
                    return RedirectToAction("Upload", "Evidence");
                }

                return RedirectToAction("Confirm","Amendments");
            }

            viewModel.PupilDetails = new PupilViewModel(amendment.Pupil);
            return View(viewModel);
        }
    }
}
