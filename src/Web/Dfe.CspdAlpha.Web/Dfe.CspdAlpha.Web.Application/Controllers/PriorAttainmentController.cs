using System.Collections.Generic;
using System.Linq;
using Dfe.CspdAlpha.Web.Application.Application;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Results;
using Dfe.Rscd.Web.ApiClient;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class PriorAttainmentController : Controller
    {
        private CheckingWindow CheckingWindow => CheckingWindowHelper.GetCheckingWindow(RouteData);

        public IActionResult Add()
        {
            var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            var amendmentDetail = amendment?.AmendmentDetail;
            if (amendment?.Pupil == null || amendmentDetail == null || amendmentDetail.GetField<string>("AddReason") != AddReason.New)
            {
                return RedirectToAction("Index", "AddPupil");
            }

            var priorAttainmentList = amendmentDetail.GetList<PriorAttainmentResult>("PriorAttainmentResults");

            if (priorAttainmentList.Count == 3)
            {
                string referer = HttpContext.Request.Headers["Referer"].ToString();
                var action = referer.Contains("AddEvidence") ? "Add" : "AddEvidence";
                return RedirectToAction(action, "Pupil");
            }
            var model = new PriorAttainmentResultViewModel
            {
                PupilDetails = new PupilViewModel(amendment.Pupil, CheckingWindow),
                Ks2Subjects = priorAttainmentList
                    .Select(r => r.Ks2Subject).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(PriorAttainmentResultViewModel result)
        {
            var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);

            var amendmentDetail = amendment.AmendmentDetail ?? new AmendmentDetail();
            if (ModelState.IsValid)
            {
                if (amendmentDetail.Fields.All(x => x.Name != "PriorAttainmentResults"))
                {
                    amendmentDetail.AddField("PriorAttainmentResults", new List<PriorAttainmentResult>());
                }

                var priorAttainmentResults = amendmentDetail.GetList<PriorAttainmentResult>("PriorAttainmentResults");

                priorAttainmentResults.Add(new PriorAttainmentResult
                {
                    Ks2Subject = result.Subject,
                    ExamYear = result.ExamYear,
                    Mark = result.TestMark,
                    ScaledScore = result.ScaledScore
                });


                HttpContext.Session.Set(Constants.AMENDMENT_SESSION_KEY, amendment);

                var results = amendmentDetail.GetList<PriorAttainmentResult>("PriorAttainmentResults");

                if (results.Count == 3)
                {
                    return RedirectToAction("Index", "Evidence");
                }

                ModelState.Clear();

                var model = new PriorAttainmentResultViewModel
                {
                    PupilDetails = new PupilViewModel(amendment.Pupil, CheckingWindow),
                    Ks2Subjects = results.Select(r => r.Ks2Subject).ToList()
                };
                return View(model);
            }

            return View(result);
        }

        public new IActionResult View()
        {
            var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            var amendmentDetail = amendment.AmendmentDetail ?? new AmendmentDetail();
            if (amendment.Pupil == null || amendmentDetail.GetField<string>("AddReason") != AddReason.Existing)
            {
                return RedirectToAction("Index", "AddPupil");
            }

            return View(new ExistingResultsViewModel(amendmentDetail.GetList<PriorAttainmentResult>("PriorAttainmentResults"),
                new PupilViewModel(amendment.Pupil, CheckingWindow)));
        }

        public IActionResult Edit()
        {
            var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            var amendmentDetail = amendment.AmendmentDetail ?? new AmendmentDetail();
            if (amendment.Pupil == null || amendmentDetail.GetField<string>("AddReason") != AddReason.Existing)
            {
                return RedirectToAction("Index", "AddPupil");
            }
            return View(new ExistingResultsViewModel(amendmentDetail.GetList<PriorAttainmentResult>("PriorAttainmentResults"),
                new PupilViewModel(amendment.Pupil, CheckingWindow)));
        }

        [HttpPost]
        public IActionResult Edit(ExistingResultsViewModel results)
        {
            var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            var amendmentDetail = amendment.AmendmentDetail ?? new AmendmentDetail();

            amendmentDetail.SetField("PriorAttainmentResults", new List<PriorAttainmentResult>
            {
                results.Reading,
                results.Writing,
                results.Maths
            });

            HttpContext.Session.Set(Constants.AMENDMENT_SESSION_KEY, amendment);
            return RedirectToAction("Index", "Evidence");
        }
    }
}
