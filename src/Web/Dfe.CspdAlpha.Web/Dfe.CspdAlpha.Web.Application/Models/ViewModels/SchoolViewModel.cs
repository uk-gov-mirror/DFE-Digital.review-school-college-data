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
        public SchoolDetails SchoolDetails { get; set; }
        public List<Pupil> PupilList { get; set; }
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
                PupilList = new List<Pupil>
                {
                    new Pupil{FirstName = "Name", LastName = "1", PupilId = "1"},
                    new Pupil{FirstName = "Name", LastName = "2", PupilId = "2"},
                    new Pupil{FirstName = "Name", LastName = "3", PupilId = "3"},
                    new Pupil{FirstName = "Name", LastName = "4", PupilId = "4"}
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
