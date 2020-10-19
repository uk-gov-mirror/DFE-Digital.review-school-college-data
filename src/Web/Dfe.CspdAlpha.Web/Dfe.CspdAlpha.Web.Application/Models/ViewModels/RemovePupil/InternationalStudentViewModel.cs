using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Common;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil
{
    public class InternationalStudentViewModel
    {
        public PupilDetails PupilDetails { get; set; }
        public string CountryOfOrigin { get; set; }
        public string Language { get; set; }
        public DateViewModel DateOfArrival { get; set; }
        public DateViewModel DateOfAdmission { get; set; }
    }
}
