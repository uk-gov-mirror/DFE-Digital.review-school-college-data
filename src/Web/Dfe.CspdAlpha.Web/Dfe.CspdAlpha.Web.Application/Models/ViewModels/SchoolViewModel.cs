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

        public static SchoolViewModel DummyData()
        {
            return new SchoolViewModel
            {
                SchoolDetails = new SchoolDetails
                {
                    SchoolName = "Lorem ipsum...",
                    URN = "123456"
                },
                CohortMeasures = new List<Measure>
                {
                    new Measure{Name = "Cohort Measure 1", Data = "1"},
                    new Measure{Name = "Cohort Measure 2", Data = "2"},
                    new Measure{Name = "Cohort Measure 3", Data = "3"},
                    new Measure{Name = "Cohort Measure 4", Data = "4"},
                },
                HeadlineMeasures = new List<Measure>
                {
                    new Measure{Name = "Headline Measure 1", Data = "1"},
                    new Measure{Name = "Headline Measure 2", Data = "2"},
                    new Measure{Name = "Headline Measure 3", Data = "3"},
                    new Measure{Name = "Headline Measure 4", Data = "4"},
                },
                AdditionalMeasures = new List<Measure>
                {
                    new Measure{Name = "Additional Measure 1", Data = "1"},
                    new Measure{Name = "Additional Measure 2", Data = "2"},
                    new Measure{Name = "Additional Measure 3", Data = "3"},
                    new Measure{Name = "Additional Measure 4", Data = "4"},
                }
            };
        }
    }
}
