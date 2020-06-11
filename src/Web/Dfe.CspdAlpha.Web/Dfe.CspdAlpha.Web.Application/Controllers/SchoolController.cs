using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class SchoolController : Controller
    {
        public IActionResult Index()
        {
            return View(SchoolViewModel.DummyData());
        }
    }
}