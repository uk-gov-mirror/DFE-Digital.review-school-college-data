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
            var countries = _dataService.GetAnswerPotentials(nameof(PupilCountryQuestion));
            var languages = _dataService.GetAnswerPotentials(nameof(PupilNativeLanguageQuestion));

            var nativeLanguageQuestion = new PupilNativeLanguageQuestion(languages.ToList());
            var countryQuestion = new PupilCountryQuestion(countries.ToList());
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