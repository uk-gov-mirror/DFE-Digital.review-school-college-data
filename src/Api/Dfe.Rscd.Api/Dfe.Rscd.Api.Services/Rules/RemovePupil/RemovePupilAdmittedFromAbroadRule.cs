using System;
using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Entities.Questions;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class RemovePupilAdmittedFromAbroadRule : RemovePupilRule
    {
        private readonly IDataService _dataService;

        public RemovePupilAdmittedFromAbroadRule(IDataService dataService)
        {
            _dataService = dataService;
        }

        public override List<Question> GetQuestions()
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
            var languages = _dataService.GetLanguages().ToList()
                .Select(x => new AnswerPotential { Value = x.Code, Description = x.Description }).ToList();

            var nativeLanguageQuestion = new PupilNativeLanguageQuestion(languages);
            var countryQuestion = new PupilCountryQuestion(countries);
            var pupilArrivalToUk = new ArrivalDateQuestion();

            return new List<Question> {nativeLanguageQuestion, countryQuestion, pupilArrivalToUk};
        }

        public override int AmendmentReason => (int)AmendmentReasonCode.AdmittedFromAbroadWithEnglishNotFirstLanguageCode;

        protected override AmendmentOutcome ApplyRule(Amendment amendment)
        {
            // Business Rules

            return new AmendmentOutcome(null);
        }
    }
}