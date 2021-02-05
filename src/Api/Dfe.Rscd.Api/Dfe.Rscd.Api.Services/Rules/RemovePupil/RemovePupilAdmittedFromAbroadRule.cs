using System;
using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.Domain.Common;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Entities.Questions;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Config;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class RemovePupilAdmittedFromAbroadRule : RemovePupilRule
    {
        private readonly IDataService _dataService;
        private readonly IAllocationYearConfig _config;

        public const int ScrutinyCode = 2;

        public RemovePupilAdmittedFromAbroadRule(IDataService dataService, IAllocationYearConfig config)
        {
            _dataService = dataService;
            _config = config;
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
            var answers = amendment.Answers;
            var countryAnswer = answers
                .Single(x => x.QuestionId == nameof(PupilCountryQuestion));
            var languageAnswer = answers
                .Single(x => x.QuestionId == nameof(PupilNativeLanguageQuestion));
            var arrivalDate = answers
                .Single(x => x.QuestionId == nameof(ArrivalDateQuestion));

            var admissionDate = amendment.Pupil.AdmissionDate;
            var hasKs2Result = amendment.Pupil.Results.Any(x => x.Qualification.ToLower() == "ks2"); // TODO CHECK
            var annualCensusDate = _config.CensusDate.ToDateTimeWhenSureNotNull("dd/MM/yyyy");
            var twoYearsAgo = DateTime.Now.AddYears(-1);
            var currentAttainmentLevel2 =
                amendment.Pupil.Results.Any(x => x.SubjectCode == "LEV2EM" && x.TestMark == "1"); // TODO CHECK
            var firstLanguage = amendment.Pupil.FirstLanguage;

            var studentCountryOfOrigin = _dataService.GetAnswerPotentials(nameof(PupilCountryQuestion))
                .Single(x => x.Value == countryAnswer.Value);

            if (studentCountryOfOrigin.Reject)
            {
                return new AmendmentOutcome(OutcomeStatus.AutoReject, "The country not on the accept list")
                {
                    ScrutinyStatusCode = ScrutinyCode.ToString(),
                    ReasonId = (int) AmendmentReasonCode.AdmittedFromAbroadWithEnglishNotFirstLanguageCode
                };
            }

            if (admissionDate < twoYearsAgo)
            {
                return new AmendmentOutcome(OutcomeStatus.AutoReject, "Admission date is before 1st June  + (CurrentYear - 2)")
                {
                    ScrutinyStatusCode = ScrutinyCode.ToString(),
                    ReasonId = (int) AmendmentReasonCode.AdmittedFromAbroadWithEnglishNotFirstLanguageCode
                };
            }

            if (hasKs2Result)
            {
                return new AmendmentOutcome(OutcomeStatus.AutoReject, "Prior key stage test results found.")
                {
                    ScrutinyStatusCode = ScrutinyCode.ToString(),
                    ReasonId = (int) AmendmentReasonCode.AdmittedFromAbroadWithEnglishNotFirstLanguageCode
                };
            }

            if (admissionDate > annualCensusDate)
            {
                return new AmendmentOutcome(OutcomeStatus.AutoReject, "The admission date is after January census.")
                {
                    ScrutinyStatusCode = ScrutinyCode.ToString(),
                    ReasonId = (int) AmendmentReasonCode.AdmittedFromAbroadWithEnglishNotFirstLanguageCode
                };
            }

            var studentLanguage = _dataService.GetAnswerPotentials(nameof(PupilNativeLanguageQuestion))
                .Single(x => x.Value == languageAnswer.Value);

            if (studentLanguage.Reject)
            {
                return new AmendmentOutcome(OutcomeStatus.AutoReject, "The language is not on the accept list.")
                {
                    ScrutinyStatusCode = ScrutinyCode.ToString(),
                    ReasonId = (int) AmendmentReasonCode.AdmittedFromAbroadWithEnglishNotFirstLanguageCode
                };
            }

            var twoYearsBeforeAnnualCensusDate = annualCensusDate.AddYears(-2);
            var studentArrivalDate = arrivalDate.Value.ToDateTime("dd/MM/yyyy");
            if (studentArrivalDate.HasValue && studentArrivalDate.Value < twoYearsBeforeAnnualCensusDate)
            {
                return new AmendmentOutcome(OutcomeStatus.AutoReject, "UK Arrival Date more than two years before ASC date.")
                {
                    ScrutinyStatusCode = ScrutinyCode.ToString(),
                    ReasonId = (int) AmendmentReasonCode.AdmittedFromAbroadWithEnglishNotFirstLanguageCode
                };
            }

            if (currentAttainmentLevel2)
            {
                return new AmendmentOutcome(OutcomeStatus.AutoReject, "The current attainment is at level 2 including English and Maths. ")
                {
                    ScrutinyStatusCode = ScrutinyCode.ToString(),
                    ReasonId = (int) AmendmentReasonCode.AdmittedFromAbroadWithEnglishNotFirstLanguageCode
                };
            }

            if (firstLanguage.Code == "ENG" || firstLanguage.Code == "ENB")
            {
                return new AmendmentOutcome(OutcomeStatus.AwatingDfeReview, "First Language Code is ENG or ENB.")
                {
                    ScrutinyStatusCode = ScrutinyCode.ToString(),
                    ReasonId = (int) AmendmentReasonCode.AdmittedFromAbroadWithEnglishNotFirstLanguageCode
                };
            }


            return new AmendmentOutcome(OutcomeStatus.AutoAccept)
            {
                ScrutinyStatusCode = ScrutinyCode.ToString(),
                ReasonId = (int) AmendmentReasonCode.AdmittedFromAbroadWithEnglishNotFirstLanguageCode
            };
        }
    }
}