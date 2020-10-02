using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.Rscd.Web.ApiClient;
using System.Linq;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Models.School;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using System.Collections.Generic;

namespace Dfe.CspdAlpha.Web.Application.Application.Services
{
    public class EstablishmentService : IEstablishmentService
    {
        private enum MeasureType
        {
            Headline,
            Additional,
            Cohort
        }

        private IClient _apiClient;

        public EstablishmentService(IClient apiClient)
        {
            _apiClient = apiClient;
        }

        private Dictionary<string, string> GetMeasures(CheckingWindow checkingWindow, MeasureType measureType)
        {
            if (checkingWindow.ToString().StartsWith("KS4"))
            {
                switch (measureType)
                {
                    case MeasureType.Headline:
                        return new Dictionary<string, string>
                        {
                            {"P8MEA", "Progress 8 measure after adjustment for extreme scores" },
                            {"ATT8SCR", "Average Attainment 8 score per pupil" },
                            {"PTEBACC_95", "Percentage of pupils achieving the English Baccalaureate with 9-5 passes" }
                        };
                    case MeasureType.Additional:
                        return new Dictionary<string, string>
                        {
                            {"EBACCAPS", "Average EBacc APS score per pupil" },
                            {"PTEBACC", "Percentage of pupils achieving the English Baccalaureate with 9-4 passes" }
                        };
                    case MeasureType.Cohort:
                    default:
                        return new Dictionary<string, string>
                        {
                            {"TPUP", "Number of pupils at the end of key stage 4"},
                            {"P8PUP", "Number of pupils included in Progress 8 measure"},
                            {
                                "SENSE4",
                                "Number of pupils at the end of key stage 4 with special educational needs (SEN) with a statement or Education, health and care (EHC) plan"
                            }
                        };

                }
            }
            if (checkingWindow.ToString().StartsWith("KS5"))
            {
                switch (measureType)
                {
                    case MeasureType.Headline:
                        return new Dictionary<string, string>
                        {
                            {"TALLPPE_ALEV_1618", "A level average result point score" },
                            {"TALLPPE_ACAD_1618", "Academic exam average result point score" },
                            {"TALLPPE_AGEN", "Applied general exam average result point score" }
                        };
                    case MeasureType.Additional:
                        return new Dictionary<string, string>
                        {
                            {"PROGEX_E", "English progress score" },
                            {"PROGEX_M", "Maths progress score" }
                        };
                    case MeasureType.Cohort:
                    default:
                        return new Dictionary<string, string>
                        {
                            {"TALLPUP_ALEV_1618", "Number of students with an A level exam entry"},
                            {"TALLPUP_ACAD_1618", "Number of students with an academic exam entry"},
                            {
                                "TALLPUP_AGEN",
                                "Number of students with an applied general exam entry"
                            }
                        };
                }
            }

            return null;
        }

        public SchoolViewModel GetSchoolViewModel(CheckingWindow checkingWindow, string urn)
        {
            var establishmentData = GetEstablishmentData(checkingWindow, urn);
            var headlineFields = GetMeasures(checkingWindow, MeasureType.Headline);
            var additionalFields = GetMeasures(checkingWindow, MeasureType.Additional);
            var cohortFields = GetMeasures(checkingWindow, MeasureType.Cohort);
            return new SchoolViewModel
            {
                SchoolDetails = new SchoolDetails
                {
                    SchoolName = establishmentData.Name,
                    URN = urn,
                    LAEstab = establishmentData.LaEstab,
                    SchoolType = establishmentData.SchoolType
                },
                HeadlineMeasures = establishmentData.PerformanceMeasures.Where(p => headlineFields.Any(h => h.Key == p.Name)).Select(m => new Measure { Name = headlineFields[m.Name], Data = m.Value }).OrderBy(s => s.Name).ToList(),
                AdditionalMeasures = establishmentData.PerformanceMeasures.Where(p => additionalFields.Any(h => h.Key == p.Name)).Select(m => new Measure { Name = additionalFields[m.Name], Data = m.Value }).OrderBy(s => s.Name).ToList(),
                CohortMeasures = establishmentData.PerformanceMeasures.Where(p => cohortFields.Any(h => h.Key == p.Name)).Select(m => new Measure { Name = cohortFields[m.Name], Data = m.Value }).OrderBy(s => s.Name).ToList()
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
