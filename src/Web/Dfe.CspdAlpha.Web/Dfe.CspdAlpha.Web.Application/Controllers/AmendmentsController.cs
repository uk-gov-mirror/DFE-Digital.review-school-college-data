using System;
using Dfe.CspdAlpha.Web.Application.Application.Extensions;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.Amendments;
using Dfe.CspdAlpha.Web.Application.Models.Amendments.AmendmentTypes;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class AmendmentsController : Controller
    {
        private readonly IAmendmentService _amendmentService;

        public AmendmentsController(IAmendmentService amendmentService, IConfiguration configuration)
        {
            _amendmentService = amendmentService;
        }

        public IActionResult Index(string urn)
        {
            var checkingWindow = CheckingWindowHelper.GetCheckingWindow(RouteData);
            var amendments = _amendmentService.GetAmendmentsListViewModel(urn, checkingWindow);
            amendments.CheckingWindow = checkingWindow;
            return View(amendments);
        }

        public IActionResult Cancel(string id)
        {
            if (_amendmentService.CancelAmendment(id))
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Error", "Home");
        }

        [ActionName("View")]
        public IActionResult ViewAmendment(string id)
        {
            var amendment = _amendmentService.GetAddPupilAmendmentViewModel(new Guid(id));
            if (amendment != null)
            {
                return View(amendment);
            }
            return RedirectToAction("Error", "Home");
        }

        public IActionResult Confirm()
        {
            return View(GetConfirmViewModel());
        }

        [HttpPost]
        public IActionResult Confirm(ConfirmViewModel viewModel)
        {
            // Ensure steps haven't been manually skipped
            //var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>(ADD_PUPIL_AMENDMENT);
            //if (addPupilAmendment == null || addPupilAmendment.SelectedEvidenceOption == EvidenceOption.Unknown)
            //{
            //    return RedirectToAction("Add");
            //}

            // Cancel amendment
            if (!viewModel.ConfirmAmendment)
            {
                // Cancel amendment
                HttpContext.Session.Remove(Constants.AMENDMENT_SESSION_KEY);
                return RedirectToAction("Index", "TaskList");
            }

            // Create amendment and redirect to amendment received page
            if (_amendmentService.CreateAddPupilAmendment(addPupilAmendment, out string id))
            {
                HttpContext.Session.Remove(Constants.AMENDMENT_SESSION_KEY);
                HttpContext.Session.Set(Constants.NEW_AMENDMENT_ID, id);
                return RedirectToAction("Received");
            }

            return View(GetConfirmViewModel());
        }

        private ConfirmViewModel GetConfirmViewModel()
        {
            var serializedAmendment = HttpContext.Session.GetString(Constants.AMENDMENT_SESSION_KEY);
            if (serializedAmendment.Contains("RemovePupil"))
            {
                var amendment = JsonConvert.DeserializeObject<Amendment<RemovePupil>>(serializedAmendment, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
                return new ConfirmViewModel{PupilDetails = amendment.AmendmentDetail.PupilDetails, AmendmentType = amendment.AmendmentDetail.AmendmentType};
            }
            return null;
        }

        public IActionResult Received()
        {
            var addPupilAmendmentId = HttpContext.Session.Get<string>(Constants.NEW_AMENDMENT_ID);
            return View("AmendmentReceived", addPupilAmendmentId);
        }
    }
}
