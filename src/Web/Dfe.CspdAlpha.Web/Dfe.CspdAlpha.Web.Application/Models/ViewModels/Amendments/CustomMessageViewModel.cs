using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments
{
    public class CustomMessageViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public PupilViewModel PupilDetails { get; set; }
    }
}
