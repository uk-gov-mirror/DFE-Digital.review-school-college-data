using System.Collections.Generic;
using Dfe.Rscd.Web.Application.Models.Common;
using Dfe.Rscd.Web.Application.Models.School;

namespace Dfe.Rscd.Web.Application.Models.ViewModels
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
