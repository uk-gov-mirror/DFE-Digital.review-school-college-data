using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.School;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels
{
    public class SchoolViewModel
    {
        public CheckDataNavigation CheckDataNavigationModel => new CheckDataNavigation(NavigationItem.SchoolPerformance);
        public ConfirmDataBanner ConfirmDataBanner { get; set; }
        public SchoolDetails SchoolDetails { get; set; }
        public List<Measure> CohortMeasures { get; set; }
        public List<Measure> HeadlineMeasures { get; set; }
        public List<Measure> AdditionalMeasures { get; set; }
    }
}
