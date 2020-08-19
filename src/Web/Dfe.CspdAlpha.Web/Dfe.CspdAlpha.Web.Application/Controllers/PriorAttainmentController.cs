using System.Linq;
using Dfe.CspdAlpha.Web.Application.Application.Extensions;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Results;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class PriorAttainmentController : Controller
    {
        private const string ADD_PUPIL_AMENDMENT = "add-pupil-amendment";

        public IActionResult Add()
        {
            var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>(ADD_PUPIL_AMENDMENT);
            if (addPupilAmendment == null || addPupilAmendment.AddPupilViewModel == null ||
                addPupilAmendment.AddReason != AddReason.New)
            {
                return RedirectToAction("Add", "Pupil");
            }
            var model = new PriorAttainmentResultViewModel
            {
                AddPupilViewModel = addPupilAmendment.AddPupilViewModel,
                Ks2Subjects = addPupilAmendment.Results.Select(r => r.Subject).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(PriorAttainmentResultViewModel result)
        {
            var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>(ADD_PUPIL_AMENDMENT);
            if (ModelState.IsValid)
            {
                addPupilAmendment.Results.Add(result);
                HttpContext.Session.Set(ADD_PUPIL_AMENDMENT, addPupilAmendment);
                ModelState.Clear();
                var model = new PriorAttainmentResultViewModel
                {
                    AddPupilViewModel = addPupilAmendment.AddPupilViewModel,
                    Ks2Subjects = addPupilAmendment.Results.Select(r => r.Subject).ToList()
                };
                return View(model);
            }
            return View(result);
        }

        public IActionResult View()
        {
            var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>(ADD_PUPIL_AMENDMENT);
            if (addPupilAmendment == null || addPupilAmendment.AddPupilViewModel == null ||
                addPupilAmendment.AddReason != AddReason.Existing)
            {
                return RedirectToAction("Add", "Pupil");
            }

            return View(new ExistingResultsViewModel(addPupilAmendment.Results, addPupilAmendment.AddPupilViewModel));
        }
    }
}
