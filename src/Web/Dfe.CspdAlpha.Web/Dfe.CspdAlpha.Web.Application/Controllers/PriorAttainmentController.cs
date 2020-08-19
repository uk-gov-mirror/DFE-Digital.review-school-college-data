using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.CspdAlpha.Web.Application.Application.Extensions;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class PriorAttainmentController : Controller
    {
        private const string ADD_PUPIL_AMENDMENT = "add-pupil-amendment";
        public PriorAttainmentController()
        {
            
        }

        public IActionResult Add()
        {
            var addPupilAmendment = HttpContext.Session.Get<AddPupilAmendmentViewModel>(ADD_PUPIL_AMENDMENT);
            if (addPupilAmendment == null || addPupilAmendment.AddPupilViewModel == null || addPupilAmendment.AddReason != AddReason.New)
            {
                return RedirectToAction("Add", "Pupil");
            }

            return View(addPupilAmendment);
        }
    }
}
