using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class AmendmentsController : Controller
    {
        private readonly IAmendmentService _amendmentService;

        public AmendmentsController(IAmendmentService amendmentService)
        {
            _amendmentService = amendmentService;
        }

        public IActionResult Index(string urn)
        {

            var amendments = _amendmentService.GetAmendmentsListViewModel(urn);
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
    }
}
