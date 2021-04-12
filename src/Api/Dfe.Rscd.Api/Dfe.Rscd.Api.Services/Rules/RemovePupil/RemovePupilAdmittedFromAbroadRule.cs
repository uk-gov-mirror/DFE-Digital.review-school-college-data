using System;
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

        protected override AmendmentOutcome ApplyRule(Amendment amendment)
        {
            var admissionDate = amendment.Pupil.AdmissionDate;
            var hasKs2Result = amendment.Pupil.Results.Any(x =>
                string.Equals(x.QualificationTypeCode, "ks2", StringComparison.InvariantCultureIgnoreCase));
            
            var annualCensusDate = _config.CensusDate.ToDateTimeWhenSureNotNull();
            var twoYearsAgo = new DateTime(DateTime.Now.AddYears(-2).Year, 6, 1);
            //var currentAttainmentLevel2 =
            //    amendment.Pupil.Results.Any(x => x.SubjectCode == "LEV2EM" && x.TestMark == "1"); // TODO: Implement when ready
            var firstLanguage = amendment.Pupil.FirstLanguage;

            var studentCountryOfOrigin = GetSelectedAnswerItem(amendment, nameof(PupilCountryQuestion));

            if (studentCountryOfOrigin.Reject)
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

            var studentLanguage = GetSelectedAnswerItem(amendment, nameof(PupilNativeLanguageQuestion));

            if (studentLanguage.Reject)
            {
                return new AmendmentOutcome(OutcomeStatus.AutoReject, "The language is not on the accept list.")
                {
                    ScrutinyStatusCode = ScrutinyCode.ToString(),
                    ReasonId = (int) AmendmentReasonCode.AdmittedFromAbroadWithEnglishNotFirstLanguageCode,
                    ReasonDescription = ReasonDescription
                };
            }

            var arrivalDate = GetAnswer(amendment, nameof(ArrivalDateQuestion));

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

            if (firstLanguage.Code == "ENG" || firstLanguage.Code == "ENB")
            {
                return new AmendmentOutcome(OutcomeStatus.AwatingDfeReview, null)
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

        protected override void ApplyOutcomeToAmendment(Amendment amendment, AmendmentOutcome amendmentOutcome)
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
                    GetFlattenedDisplayField(amendment, nameof(PupilCountryQuestion)));

                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_NativeLanguage, 
                    GetFlattenedDisplayField(amendment, nameof(PupilNativeLanguageQuestion)));

                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_DateOfArrivalUk, 
                    GetAnswer(amendment, nameof(ArrivalDateQuestion)).Value);
  
                if (isNonPlasc)
                {
                    amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_DateOnRoll, 
                        GetAnswer(amendment, nameof(PupilDateOnRollQuestion)).Value);
                }
            }
        }
    }
}