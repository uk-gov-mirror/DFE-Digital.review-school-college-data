using System.Collections.Generic;
using System.Linq;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.School;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using Dfe.CspdAlpha.Web.Domain.Core;
using Dfe.CspdAlpha.Web.Domain.Interfaces;

namespace Dfe.CspdAlpha.Web.Application.Application.Services
{
    public class SchoolService : ISchoolService
    {
        private IEstablishmentService _establishmentService;
        private readonly List<string> HEADLINE_MEASURES = new List<string> { "P8_BANDING", "PTEBACC_E_PTQ_EE", "PTL2BASICS_95" };
        private readonly List<string> ADDITIONAL_MEASURES = new List<string> { "PTL2BASICS_94", "ATT8SCR", "EBACCAPS" };

        public SchoolService(IEstablishmentService establishmentService)
        {
            _establishmentService = establishmentService;
        }

        public SchoolViewModel GetSchoolViewModel(string urn)
        {
            var establishmentData = _establishmentService.GetByURN(new URN(urn));
            return new SchoolViewModel
            {
                SchoolDetails = new SchoolDetails
                {
                    SchoolName = establishmentData.Name,
                    URN = urn
                },
                HeadlineMeasures = establishmentData.PerformanceMeasures.Where(p => HEADLINE_MEASURES.Any(h => h == p.Name)).Select(m => new Measure{Name = m.Name, Data = m.Value}).ToList(),
                AdditionalMeasures = establishmentData.PerformanceMeasures.Where(p => ADDITIONAL_MEASURES.Any(h => h == p.Name)).Select(m => new Measure{Name = m.Name, Data = m.Value}).ToList(),
                PupilList = new List<Pupil>
                {
                    new Pupil{FirstName = "Name", LastName = "1", PupilId = "1"},
                    new Pupil{FirstName = "Name", LastName = "2", PupilId = "2"},
                    new Pupil{FirstName = "Name", LastName = "3", PupilId = "3"},
                    new Pupil{FirstName = "Name", LastName = "4", PupilId = "4"}
                }
            };
        }
    }
}
