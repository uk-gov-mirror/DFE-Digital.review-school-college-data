using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.Domain.Common;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Entities.Amendments;
using Dfe.Rscd.Api.Domain.Entities.Questions;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Config;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class RemovePupilOtherEalExceptionalCircumstances : Rule
    {
        private readonly IDataService _dataService;
        private readonly IEstablishmentService _establishmentService;
        private readonly IAllocationYearConfig _config;
        
        private const string ReasonDescription = "Other - EAL exceptional circumstances";
        private string _evidenceHelpDeskText => Content.RemovePupilOtherEalExceptionalCirumstances_HTML;

        public RemovePupilOtherEalExceptionalCircumstances(IDataService dataService, IAllocationYearConfig config, IEstablishmentService establishmentService)
        {
            _dataService = dataService;
            _establishmentService = establishmentService;
            _config = config;
        }
        public override AmendmentType AmendmentType => AmendmentType.RemovePupil;
        public override int AmendmentReason => (int) AmendmentReasonCode.OtherEAL;
        
        public override List<Question> GetQuestions(Amendment amendment)
        {
            var questions = new List<Question>();
            var countries = _dataService.GetAnswerPotentials(nameof(PupilCountryQuestion));
            var languages = _dataService.GetAnswerPotentials(nameof(PupilNativeLanguageQuestion));
            var isNonPlasc = _establishmentService.IsNonPlascEstablishment(amendment.CheckingWindow, new URN(amendment.URN));
            var nativeLanguageQuestion = new PupilNativeLanguageQuestion(languages.ToList());
            questions.Add(nativeLanguageQuestion);
            
            var countryQuestion = new PupilCountryQuestion(countries.ToList());
            questions.Add(countryQuestion);
            
            var pupilArrivalToUk = new ArrivalDateQuestion();
            questions.Add(pupilArrivalToUk);

            if (isNonPlasc)
            {
                questions.Add(new PupilDateOnRollQuestion());
            }
            
            var evidenceQuestion = new EvidenceUploadQuestion(_evidenceHelpDeskText);
            questions.Add(evidenceQuestion);

            return questions;
        }

        protected override AmendmentOutcome ApplyRule(Amendment amendment)
        {
            var arrivalDate = GetAnswer(amendment, nameof(ArrivalDateQuestion));
            var evidenceUploadQuestion = GetAnswer(amendment, nameof(EvidenceUploadQuestion));

            amendment.EvidenceStatus = string.IsNullOrEmpty(evidenceUploadQuestion.Value) || evidenceUploadQuestion.Value == "0"
                ? EvidenceStatus.Later : EvidenceStatus.Now;
            
            return new AmendmentOutcome(OutcomeStatus.AwatingDfeReview, ReasonDescription)
            {
                ScrutinyStatusCode = string.Empty,
                ReasonId = (int)AmendmentReasonCode.Other,
                ReasonDescription = "Other",
                SubReason = ReasonDescription
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
                
                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_SubReasonDescription,
                    amendmentOutcome.SubReason);

                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_OutcomeDescription,
                    amendmentOutcome.OutcomeDescription);
                
                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_DateOfArrivalUk,
                    GetAnswer(amendment, nameof(ArrivalDateQuestion)).Value);
                
                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_CountryOfOrigin, 
                    GetFlattenedDisplayField(amendment, nameof(PupilCountryQuestion)));
                
                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_NativeLanguage, 
                    GetFlattenedDisplayField(amendment, nameof(PupilNativeLanguageQuestion)));

                if (isNonPlasc)
                {
                    amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_DateOnRoll, 
                        GetAnswer(amendment, nameof(PupilDateOnRollQuestion)).Value);
                }
            }
        }
    }
}