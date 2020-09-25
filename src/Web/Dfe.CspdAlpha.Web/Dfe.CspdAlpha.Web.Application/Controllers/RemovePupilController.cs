using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class RemovePupilController : Controller
    {
        private ISchoolService _schoolService;

        public RemovePupilController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
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
                var pupilsFound = _schoolService.GetPupilListViewModel(RouteData.Values["phase"].ToString(),
                    ClaimsHelper.GetURN(this.User), viewModel.PupilID, viewModel.Name);
                if (pupilsFound.Pupils.Count == 0 || pupilsFound.Pupils.Count > 1)
                {
                    return View("ResultsList", viewModel);
                }
                RedirectToAction("Pupil", new {id = pupilsFound.Pupils.First().PupilId});
            }
            return View(viewModel);
        }
    }
}
