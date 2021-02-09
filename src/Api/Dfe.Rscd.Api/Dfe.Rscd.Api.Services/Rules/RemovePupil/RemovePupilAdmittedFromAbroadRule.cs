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
        private const string ReasonDescription = "Admited from aboard with English not first language";

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

        protected override List<ValidatedAnswer> GetValidatedAnswers(List<UserAnswer> userAnswers)
        {
            var questions = GetQuestions();

            var languageQuestion = questions.Single(x => x.Id == nameof(PupilNativeLanguageQuestion));
            var countryAnswer = questions.Single(x => x.Id == nameof(PupilCountryQuestion));
            var studentArrivalDate = questions.Single(x => x.Id == nameof(ArrivalDateQuestion));

            return new List<ValidatedAnswer>
            {
                languageQuestion.GetAnswer(userAnswers),
                countryAnswer.GetAnswer(userAnswers),
                studentArrivalDate.GetAnswer(userAnswers)
            };
        }

        protected override AmendmentOutcome ApplyRule(Amendment amendment, List<ValidatedAnswer> validatedAnswers)
        {
            var admissionDate = amendment.Pupil.AdmissionDate;
            var hasKs2Result = amendment.Pupil.Results.Any(x => x.Qualification.ToLower() == "ks2"); // TODO CHECK
            var annualCensusDate = _config.CensusDate.ToDateTimeWhenSureNotNull("dd/MM/yyyy");
            var twoYearsAgo = DateTime.Now.AddYears(-1);
            var currentAttainmentLevel2 =
                amendment.Pupil.Results.Any(x => x.SubjectCode == "LEV2EM" && x.TestMark == "1"); // TODO CHECK
            var firstLanguage = amendment.Pupil.FirstLanguage;

            var studentCountryOfOrigin = validatedAnswers.Single(x => x.QuestionId == nameof(PupilCountryQuestion));

            if (studentCountryOfOrigin.IsRejected)
            {
                return new AmendmentOutcome(OutcomeStatus.AutoReject, "The country is not on the accept list")
                {
                    ScrutinyStatusCode = ScrutinyCode.ToString(),
                    ReasonId = (int) AmendmentReasonCode.AdmittedFromAbroadWithEnglishNotFirstLanguageCode,
                    ReasonDescription = ReasonDescription
                };
            }

            if (admissionDate < twoYearsAgo)
            {
                return new AmendmentOutcome(OutcomeStatus.AutoReject, "Admission date is before 1st June  + (CurrentYear - 2)")
                {
                    ScrutinyStatusCode = ScrutinyCode.ToString(),
                    ReasonId = (int) AmendmentReasonCode.AdmittedFromAbroadWithEnglishNotFirstLanguageCode,
                    ReasonDescription = ReasonDescription
                };
            }

            if (hasKs2Result)
            {
                return new AmendmentOutcome(OutcomeStatus.AutoReject, "Prior key stage test results found.")
                {
                    ScrutinyStatusCode = ScrutinyCode.ToString(),
                    ReasonId = (int) AmendmentReasonCode.AdmittedFromAbroadWithEnglishNotFirstLanguageCode,
                    ReasonDescription = ReasonDescription
                };
            }

            if (admissionDate > annualCensusDate)
            {
                return new AmendmentOutcome(OutcomeStatus.AutoReject, "The admission date is after January census.")
                {
                    ScrutinyStatusCode = ScrutinyCode.ToString(),
                    ReasonId = (int) AmendmentReasonCode.AdmittedFromAbroadWithEnglishNotFirstLanguageCode,
                    ReasonDescription = ReasonDescription
                };
            }

            var studentLanguage = validatedAnswers.Single(x => x.QuestionId == nameof(PupilNativeLanguageQuestion));

            if (studentLanguage.IsRejected)
            {
                return new AmendmentOutcome(OutcomeStatus.AutoReject, "The language is not on the accept list.")
                {
                    ScrutinyStatusCode = ScrutinyCode.ToString(),
                    ReasonId = (int) AmendmentReasonCode.AdmittedFromAbroadWithEnglishNotFirstLanguageCode,
                    ReasonDescription = ReasonDescription
                };
            }

            var arrivalDate = validatedAnswers.Single(x => x.QuestionId == nameof(ArrivalDateQuestion));

            var twoYearsBeforeAnnualCensusDate = annualCensusDate.AddYears(-2);
            var studentArrivalDate = arrivalDate.Value.ToDateTime("dd/MM/yyyy");
            if (studentArrivalDate.HasValue && studentArrivalDate.Value < twoYearsBeforeAnnualCensusDate)
            {
                return new AmendmentOutcome(OutcomeStatus.AutoReject, "UK Arrival Date more than two years before ASC date.")
                {
                    ScrutinyStatusCode = ScrutinyCode.ToString(),
                    ReasonId = (int) AmendmentReasonCode.AdmittedFromAbroadWithEnglishNotFirstLanguageCode,
                    ReasonDescription = ReasonDescription
                };
            }

            if (currentAttainmentLevel2)
            {
                return new AmendmentOutcome(OutcomeStatus.AutoReject, "The current attainment is at level 2 including English and Maths. ")
                {
                    ScrutinyStatusCode = ScrutinyCode.ToString(),
                    ReasonId = (int) AmendmentReasonCode.AdmittedFromAbroadWithEnglishNotFirstLanguageCode,
                    ReasonDescription = ReasonDescription
                };
            }

            if (firstLanguage.Code == "ENG" || firstLanguage.Code == "ENB")
            {
                return new AmendmentOutcome(OutcomeStatus.AwatingDfeReview, "First Language Code is ENG or ENB.")
                {
                    ScrutinyStatusCode = ScrutinyCode.ToString(),
                    ReasonId = (int) AmendmentReasonCode.AdmittedFromAbroadWithEnglishNotFirstLanguageCode,
                    ReasonDescription = ReasonDescription
                };
            }
            
            return new AmendmentOutcome(OutcomeStatus.AutoAccept)
            {
                ScrutinyStatusCode = ScrutinyCode.ToString(),
                ReasonId = (int) AmendmentReasonCode.AdmittedFromAbroadWithEnglishNotFirstLanguageCode,
                ReasonDescription = ReasonDescription
            };
        }
    }
}