using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Entities.Questions;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class AdmittedFromAbroadRules : RemovePupilRule
    {
        private readonly IDataService _dataService;

        public AdmittedFromAbroadRules(IDataService dataService)
        {
            _dataService = dataService;
        }

        public override int AmendmentReason => (int)AmendmentReasonCode.AdmittedFromAbroadWithEnglishNotFirstLanguageCode;

        protected override AmendmentOutcome ApplyRule(Amendment amendment)
        {
            if (amendment.IsNewAmendment)
            {
                var countries = new List<AnswerPotential>
                {
                    new AnswerPotential {Value = "1", Description = "France"},
                    new AnswerPotential {Value = "2", Description = "United States of America"},
                    new AnswerPotential {Value = "3", Description = "Nigeria"},
                    new AnswerPotential {Value = "4", Description = "South Africa"},
                    new AnswerPotential {Value = "5", Description = "India"},
                    new AnswerPotential {Value = "?", Description = "Other"}
                };
                var languages = _dataService.GetLanguages()
                    .Select(x => new AnswerPotential {Value = x.Code, Description = x.Description}).ToList();

                var countryQuestion = new PupilNativeLanguageQuestion(languages);
                var languageQuestion = new PupilCountryQuestion(countries);
                var pupilArrivalToUk = new ArrivalDateQuestion();

                return new AmendmentOutcome(new List<Question> { countryQuestion, languageQuestion, pupilArrivalToUk});
            }

            return new AmendmentOutcome(null);
        }
    }
}