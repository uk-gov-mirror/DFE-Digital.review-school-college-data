using System.Collections.Generic;
using System.Linq;
using Dfe.CspdAlpha.Web.Application.Application.Extensions;
using Dfe.CspdAlpha.Web.Application.Models.Amendments;
using Dfe.CspdAlpha.Web.Application.Models.Amendments.AmendmentTypes;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Results;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class PriorAttainmentController : Controller
    {

        public IActionResult Add()
        {
            var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            var amendmentDetail = (AddPupil) amendment?.AmendmentDetail;
            if (amendment?.PupilDetails == null || amendmentDetail == null || amendmentDetail.AddReason != AddReason.New)
            {
                return RedirectToAction("Index", "AddPupil");
            }
            if (amendmentDetail.PriorAttainmentResults.Count == 3)
            {
                string referer = HttpContext.Request.Headers["Referer"].ToString();
                var action = referer.Contains("AddEvidence") ? "Add" : "AddEvidence";
                return RedirectToAction(action, "Pupil");
            }
            var model = new PriorAttainmentResultViewModel
            {
                PupilDetails = amendment.PupilDetails,
                Ks2Subjects = amendmentDetail.PriorAttainmentResults.Select(r => r.Ks2Subject).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(PriorAttainmentResultViewModel result)
        {
            var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            var amendmentDetail = (AddPupil)amendment?.AmendmentDetail;
            if (ModelState.IsValid)
            {
                amendmentDetail.PriorAttainmentResults.Add(new PriorAttainmentResult{Ks2Subject = result.Subject, ExamYear = result.ExamYear, Mark = result.TestMark, ScaledScore = result.ScaledScore});
                HttpContext.Session.Set(Constants.AMENDMENT_SESSION_KEY, amendment);
                if (amendmentDetail.PriorAttainmentResults.Count == 3)
                {
                    return RedirectToAction("Index", "Evidence");
                }
                ModelState.Clear();
                var model = new PriorAttainmentResultViewModel
                {
                    PupilDetails = amendment.PupilDetails,
                    Ks2Subjects = amendmentDetail.PriorAttainmentResults.Select(r => r.Ks2Subject).ToList()
                };
                return View(model);
            }
            return View(result);
        }

        public new IActionResult View()
        {
            var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            var amendmentDetail = (AddPupil)amendment?.AmendmentDetail;
            if (amendment?.PupilDetails == null || amendmentDetail == null || amendmentDetail.AddReason != AddReason.Existing)
            {
                return RedirectToAction("Index", "AddPupil");
            }

            return View(new ExistingResultsViewModel(amendmentDetail.PriorAttainmentResults, amendment.PupilDetails));
        }

        public IActionResult Edit()
        {
            var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            var amendmentDetail = (AddPupil)amendment?.AmendmentDetail;
            if (amendment?.PupilDetails == null || amendmentDetail == null || amendmentDetail.AddReason != AddReason.Existing)
            {
                return RedirectToAction("Index", "AddPupil");
            }
            return View(new ExistingResultsViewModel(amendmentDetail.PriorAttainmentResults, amendment.PupilDetails));
        }

        [HttpPost]
        public IActionResult Edit(ExistingResultsViewModel results)
        {
            var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            var amendmentDetail = (AddPupil)amendment?.AmendmentDetail;
            amendmentDetail.PriorAttainmentResults = new List<PriorAttainmentResult>
            {
                results.Reading,
                results.Writing,
                results.Maths
            };
            HttpContext.Session.Set(Constants.AMENDMENT_SESSION_KEY, amendment);
            return RedirectToAction("Index", "Evidence");
        }
    }
}
