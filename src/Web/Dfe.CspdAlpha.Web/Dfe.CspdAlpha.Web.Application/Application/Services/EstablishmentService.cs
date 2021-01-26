using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using ApiClient = Dfe.Rscd.Web.ApiClient;
using System.Linq;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Models.School;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

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

        private readonly ApiClient.IClient _apiClient;
        private readonly ApiClient.CheckingWindow _checkingWindow;
        private readonly string _checkingWindowUrl;

        public EstablishmentService(ApiClient.IClient apiClient, IHttpContextAccessor httpContextAccessor)
        {
            _apiClient = apiClient;
            _checkingWindow = CheckingWindowHelper.GetCheckingWindow(httpContextAccessor.HttpContext.Request.RouteValues);
            _checkingWindowUrl = CheckingWindowHelper.GetCheckingWindowURL(_checkingWindow);
        }

        private Dictionary<string, string> GetMeasures(MeasureType measureType)
        {
            if (_checkingWindow.ToString().StartsWith("KS4"))
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
            if (_checkingWindow.ToString().StartsWith("KS5"))
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

        public SchoolViewModel GetSchoolViewModel(string urn)
        {
            var establishmentData = GetEstablishmentData(urn);
            var headlineFields = GetMeasures(MeasureType.Headline);
            var additionalFields = GetMeasures(MeasureType.Additional);
            var cohortFields = GetMeasures(MeasureType.Cohort);
            return new SchoolViewModel
            {
                SchoolDetails = new SchoolDetails
                {
                    SchoolName = establishmentData.SchoolName,
                    URN = urn,
                    DfesNumber = establishmentData.DfesNumber.ToString(),
                    SchoolType = establishmentData.SchoolType
                },
                HeadlineMeasures = establishmentData.PerformanceMeasures
                    .Where(p => headlineFields.Any(h => h.Key == p.Name))
                    .Select(m => new Measure { Name = headlineFields[m.Name], Data = m.Value })
                    .OrderBy(s => s.Name)
                    .ToList(),

                AdditionalMeasures = establishmentData.PerformanceMeasures
                    .Where(p => additionalFields.Any(h => h.Key == p.Name))
                    .Select(m => new Measure { Name = additionalFields[m.Name], Data = m.Value })
                    .OrderBy(s => s.Name)
                    .ToList(),

                CohortMeasures = establishmentData.PerformanceMeasures
                    .Where(p => cohortFields.Any(h => h.Key == p.Name))
                    .Select(m => new Measure { Name = cohortFields[m.Name], Data = m.Value })
                    .OrderBy(s => s.Name)
                    .ToList()
            };
        }
        private ApiClient.School GetEstablishmentData(string urn)
        {
            var school = _apiClient.GetEstablishmentByURNAsync(urn, _checkingWindowUrl).GetAwaiter().GetResult();
            if (string.IsNullOrWhiteSpace(school.Error.ErrorMessage))
            {
                return school.Result;
            }
            return null;
        }

        public SchoolDetails GetSchoolDetails(string urn)
        {
            var establishmentData = GetEstablishmentData(urn);
            return new SchoolDetails
            {
                SchoolName = establishmentData.SchoolName,
                URN = urn,
                DfesNumber = establishmentData.DfesNumber.ToString(),
                SchoolType = establishmentData.SchoolType
            };
        }

        public string GetSchoolName(string laestab)
        {
            var school = _apiClient.SearchTEstablishmentsAsync(laestab, _checkingWindowUrl).GetAwaiter().GetResult();
            return school != null ? school.Result.SchoolName : string.Empty;
        }
    }
}
