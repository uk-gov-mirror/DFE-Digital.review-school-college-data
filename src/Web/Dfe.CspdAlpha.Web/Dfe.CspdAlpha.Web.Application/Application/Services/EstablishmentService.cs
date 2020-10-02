using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Establishments;
using Dfe.Rscd.Web.ApiClient;
using System;
using System.Linq;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Models.School;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using System.Collections.Generic;

namespace Dfe.CspdAlpha.Web.Application.Application.Services
{
    public class EstablishmentService : IEstablishmentService
    {
        private IClient _apiClient;
        private readonly Dictionary<string, string> HEADLINE_SCORES = new Dictionary<string, string>
        {
            {"P8MEA", "Progress 8 measure after adjustment for extreme scores" },
            {"ATT8SCR", "Average Attainment 8 score per pupil" },
            {"PTEBACC_95", "Percentage of pupils achieving the English Baccalaureate with 9-5 passes" }
        };
        private readonly Dictionary<string, string> ADDITIONAL_SCORES = new Dictionary<string, string>
        {
            {"EBACCAPS", "Average EBacc APS score per pupil" },
            {"PTEBACC", "Percentage of pupils achieving the English Baccalaureate with 9-4 passes" }
        };
        private readonly Dictionary<string, string> COHORT_SCORES = new Dictionary<string, string>
        {
            {"TPUP", "Number of pupils at the end of key stage 4" },
            {"P8PUP", "Number of pupils included in Progress 8 measure" },
            {"SENSE4", "Number of pupils at the end of key stage 4 with special educational needs (SEN) with a statement or Education, health and care (EHC) plan" }
        };

        public EstablishmentService(IClient apiClient)
        {
            _apiClient = apiClient;
        }

        public SchoolViewModel GetSchoolViewModel(CheckingWindow checkingWindow, string urn)
        {
            var establishmentData = GetEstablishmentData(checkingWindow, urn);
            return new SchoolViewModel
            {
                SchoolDetails = new SchoolDetails
                {
                    SchoolName = establishmentData.Name,
                    URN = urn,
                    LAEstab = establishmentData.LaEstab,
                    SchoolType = establishmentData.SchoolType
                },
                HeadlineMeasures = establishmentData.PerformanceMeasures.Where(p => HEADLINE_SCORES.Any(h => h.Key == p.Name)).Select(m => new Measure { Name = HEADLINE_SCORES[m.Name], Data = m.Value }).OrderBy(s => s.Name).ToList(),
                AdditionalMeasures = establishmentData.PerformanceMeasures.Where(p => ADDITIONAL_SCORES.Any(h => h.Key == p.Name)).Select(m => new Measure { Name = ADDITIONAL_SCORES[m.Name], Data = m.Value }).OrderBy(s => s.Name).ToList(),
                CohortMeasures = establishmentData.PerformanceMeasures.Where(p => COHORT_SCORES.Any(h => h.Key == p.Name)).Select(m => new Measure { Name = COHORT_SCORES[m.Name], Data = m.Value }).OrderBy(s => s.Name).ToList()
            };
        }
        private Establishment GetEstablishmentData(CheckingWindow checkingWindow, string urn)
        {
            var school = _apiClient.EstablishmentsAsync(urn, checkingWindow.ToString().ToLower()).GetAwaiter().GetResult();
            if (string.IsNullOrWhiteSpace(school.Error.ErrorMessage))
            {
                return school.Result;
            }
            return null;
        }

        public string GetSchoolName(CheckingWindow checkingWindow, string laestab)
        {
            var checkingWindowURL = CheckingWindowHelper.GetCheckingWindowURL(checkingWindow);
            var school = _apiClient.SearchAsync(laestab, checkingWindowURL).GetAwaiter().GetResult();
            return school != null ? school.Result.Name : string.Empty;
        }
    }
}
