using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.School;
using System.Collections.Generic;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels
{
    public class SchoolViewModel : ContextAwareViewModel
    {
        public CheckDataNavigation CheckDataNavigationModel => new CheckDataNavigation(NavigationItem.SchoolPerformance, CheckingWindow);
        public SchoolDetails SchoolDetails { get; set; }
        public List<Measure> CohortMeasures { get; set; }
        public List<Measure> HeadlineMeasures { get; set; }
        public List<Measure> AdditionalMeasures { get; set; }
        public string DataDescription { get; set; }
    }
}
