﻿using System;
using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.Domain.Common;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Entities.Amendments;
using Dfe.Rscd.Api.Domain.Entities.Questions;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Config;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class RemovePupilAdmittedFromAbroadRule : Rule
    {
        private readonly IDataService _dataService;
        private readonly IEstablishmentService _establishmentService;
        private readonly IAllocationYearConfig _config;
        private const string ReasonDescription = "Admitted from abroad with English not first language";

        public const int ScrutinyCode = 2;

        public RemovePupilAdmittedFromAbroadRule(IDataService dataService, IAllocationYearConfig config,
            IEstablishmentService establishmentService)
        {
            _dataService = dataService;
            _establishmentService = establishmentService;
            _config = config;
        }

        public override List<Question> GetQuestions(Amendment amendment)
        {
            var isNonPlasc = _establishmentService.IsNonPlascEstablishment(amendment.CheckingWindow, new URN(amendment.URN));
            var countries = _dataService.GetAnswerPotentials(nameof(PupilCountryQuestion));
            var languages = _dataService.GetAnswerPotentials(nameof(PupilNativeLanguageQuestion));

            var nativeLanguageQuestion = new PupilNativeLanguageQuestion(languages.ToList());
            var countryQuestion = new PupilCountryQuestion(countries.ToList());
            var pupilArrivalToUk = new ArrivalDateQuestion();
            var questions = new List<Question>()
            {
                nativeLanguageQuestion, countryQuestion, pupilArrivalToUk
            };

            if (isNonPlasc)
            {
                questions.Add(new PupilDateOnRollQuestion());
            }

            return questions;
        }

        public override int AmendmentReason => (int)AmendmentReasonCode.AdmittedFromAbroadWithEnglishNotFirstLanguageCode;
        public override AmendmentType AmendmentType => AmendmentType.RemovePupil;

        protected override List<ValidatedAnswer> GetValidatedAnswers(Amendment amendment)
        {
            var questions = GetQuestions(amendment);
            var isNonPlasc = _establishmentService.IsNonPlascEstablishment(amendment.CheckingWindow, new URN(amendment.URN));
            
            var languageQuestion = questions.Single(x => x.Id == nameof(PupilNativeLanguageQuestion));
            var countryAnswer = questions.Single(x => x.Id == nameof(PupilCountryQuestion));
            var studentArrivalDate = questions.Single(x => x.Id == nameof(ArrivalDateQuestion));

            var validatedAnswers = new List<ValidatedAnswer>()
            {
                languageQuestion.GetAnswer(amendment),
                countryAnswer.GetAnswer(amendment),
                studentArrivalDate.GetAnswer(amendment)
            };

            if (isNonPlasc)
            {
                var dateOnRollAnswer = questions.Single(x => x.Id == nameof(PupilDateOnRollQuestion));
                validatedAnswers.Add(dateOnRollAnswer.GetAnswer(amendment));
            }

            return validatedAnswers;
        }

        protected override AmendmentOutcome ApplyRule(Amendment amendment, List<ValidatedAnswer> validatedAnswers)
        {
            var admissionDate = amendment.Pupil.AdmissionDate;
            var hasKs2Result = amendment.Pupil.Results.Any(x =>
                string.Equals(x.QualificationTypeCode, "ks2", StringComparison.InvariantCultureIgnoreCase));
            
            var annualCensusDate = _config.CensusDate.ToDateTimeWhenSureNotNull();
            var twoYearsAgo = new DateTime(DateTime.Now.AddYears(-2).Year, 6, 1);
            //var currentAttainmentLevel2 =
            //    amendment.Pupil.Results.Any(x => x.SubjectCode == "LEV2EM" && x.TestMark == "1"); // TODO: Implement when ready
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
                return new AmendmentOutcome(OutcomeStatus.AutoReject, string.Concat("Admission date is before 1st June ", twoYearsAgo.Year))
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
            var studentArrivalDate = arrivalDate.Value.ToDateTime();
            if (studentArrivalDate.HasValue && studentArrivalDate.Value < twoYearsBeforeAnnualCensusDate)
            {
                return new AmendmentOutcome(OutcomeStatus.AutoReject, "UK Arrival Date more than two years before ASC date.")
                {
                    ScrutinyStatusCode = ScrutinyCode.ToString(),
                    ReasonId = (int) AmendmentReasonCode.AdmittedFromAbroadWithEnglishNotFirstLanguageCode,
                    ReasonDescription = ReasonDescription
                };
            }

            //if (currentAttainmentLevel2)
            //{
            //    return new AmendmentOutcome(OutcomeStatus.AutoReject, "The current attainment is at level 2 including English and Maths. ")
            //    {
            //        ScrutinyStatusCode = ScrutinyCode.ToString(),
            //        ReasonId = (int) AmendmentReasonCode.AdmittedFromAbroadWithEnglishNotFirstLanguageCode,
            //        ReasonDescription = ReasonDescription
            //    };
            //}

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

        protected override void ApplyOutcomeToAmendment(Amendment amendment, AmendmentOutcome amendmentOutcome, List<ValidatedAnswer> answers)
        {
            if (amendmentOutcome.IsComplete && amendmentOutcome.FurtherQuestions == null)
            {
                var isNonPlasc = _establishmentService.IsNonPlascEstablishment(amendment.CheckingWindow, new URN(amendment.URN));
                
                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_ReasonDescription,
                    amendmentOutcome.ReasonDescription);

                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_ReasonCode,
                    amendmentOutcome.ReasonId);

                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_OutcomeDescription,
                    amendmentOutcome.OutcomeDescription);

                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_CountryOfOrigin, 
                    GetAnswer(answers, nameof(PupilCountryQuestion)).Value);

                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_NativeLanguage, 
                    GetAnswer(answers, nameof(PupilNativeLanguageQuestion)).Value);

                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_DateOfArrivalUk, 
                    GetAnswer(answers, nameof(ArrivalDateQuestion)).Value);
                
                if (isNonPlasc)
                {
                    amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_DateOnRoll, 
                        GetAnswer(answers, nameof(PupilDateOnRollQuestion)).Value);
                }
            }
        }
    }
}