using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Web.ApiClient;
using Dfe.Rscd.Web.Application.Models.Common;
using Dfe.Rscd.Web.Application.Models.ViewModels.Pupil;
using Dfe.Rscd.Web.Application.Models.ViewModels.Results;
using Dfe.Rscd.Web.Application.Security;
using Microsoft.AspNetCore.Mvc;
using Constants = Dfe.Rscd.Web.Application.Models.Common.Constants;

namespace Dfe.Rscd.Web.Application.Controllers
{
    public class PriorAttainmentController : SessionController
    {
        public PriorAttainmentController(UserInfo userInfo)
            : base(userInfo)
        {

        }
        public IActionResult Add()
        {
            var amendment = GetAmendment();
            var amendmentDetail = amendment?.AmendmentDetail;
            if (amendment?.Pupil == null || amendmentDetail == null ||
                amendmentDetail.GetField<string>(Constants.AddPupil.AddReason) != AddReason.New)
                return RedirectToAction("Index", "AddPupil");

            var priorAttainmentList = amendmentDetail
                .GetList<PriorAttainmentResult>(Constants.AddPupil.PriorAttainmentResults);

            if (priorAttainmentList.Count == 3)
            {
                var referer = HttpContext.Request.Headers["Referer"].ToString();
                var action = referer.Contains("AddEvidence") ? "Add" : "AddEvidence";
                return RedirectToAction(action, "Pupil");
            }

            var model = new PriorAttainmentResultViewModel
            {
                PupilDetails = new PupilViewModel(amendment.Pupil),
                Ks2Subjects = priorAttainmentList
                    .Select(r => r.Ks2Subject).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Add(PriorAttainmentResultViewModel result)
        {
            var amendment = GetAmendment();

            var amendmentDetail = amendment.AmendmentDetail ?? new AmendmentDetail();
            if (ModelState.IsValid)
            {
                if (amendmentDetail.Fields.All(x => x.Name != Constants.AddPupil.PriorAttainmentResults))
                    amendmentDetail.AddField(Constants.AddPupil.PriorAttainmentResults,
                        new List<PriorAttainmentResult>());

                var priorAttainmentResults =
                    amendmentDetail.GetList<PriorAttainmentResult>(Constants.AddPupil.PriorAttainmentResults);

                priorAttainmentResults.Add(new PriorAttainmentResult
                {
                    Ks2Subject = result.SubjectCode,
                    ExamYear = result.ExamYear.ToString(),
                    Mark = result.TestMark,
                    ScaledScore = result.ScaledScore
                });


                SaveAmendment(amendment);

                var results = amendmentDetail
                    .GetList<PriorAttainmentResult>(Constants.AddPupil.PriorAttainmentResults);

                if (results.Count == 3) return RedirectToAction("Index", "Evidence");

                ModelState.Clear();

                var model = new PriorAttainmentResultViewModel
                {
                    PupilDetails = new PupilViewModel(amendment.Pupil),
                    Ks2Subjects = results.Select(r => r.Ks2Subject).ToList()
                };
                return View(model);
            }

            return View(result);
        }

        public new IActionResult View()
        {
            var amendment = GetAmendment();
            var amendmentDetail = amendment.AmendmentDetail ?? new AmendmentDetail();
            if (amendment.Pupil == null ||
                amendmentDetail.GetField<string>(Constants.AddPupil.AddReason) != AddReason.Existing)
                return RedirectToAction("Index", "AddPupil");

            return View(new ExistingResultsViewModel(
                amendmentDetail.GetList<PriorAttainmentResult>(Constants.AddPupil.PriorAttainmentResults),
                new PupilViewModel(amendment.Pupil)));
        }

        public IActionResult Edit()
        {
            var amendment = GetAmendment();
            var amendmentDetail = amendment.AmendmentDetail ?? new AmendmentDetail();
            if (amendment.Pupil == null ||
                amendmentDetail.GetField<string>(Constants.AddPupil.AddReason) != AddReason.Existing)
                return RedirectToAction("Index", "AddPupil");
            return View(new ExistingResultsViewModel(
                amendmentDetail.GetList<PriorAttainmentResult>(Constants.AddPupil.PriorAttainmentResults),
                new PupilViewModel(amendment.Pupil)));
        }

        [HttpPost]
        public IActionResult Edit(ExistingResultsViewModel results)
        {
            var amendment = GetAmendment();
            var amendmentDetail = amendment.AmendmentDetail ?? new AmendmentDetail();

            amendmentDetail.SetField(Constants.AddPupil.PriorAttainmentResults, new List<PriorAttainmentResult>
            {
                results.Reading,
                results.Writing,
                results.Maths
            });

            SaveAmendment(amendment);

            return RedirectToAction("Index", "Evidence");
        }
    }
}
