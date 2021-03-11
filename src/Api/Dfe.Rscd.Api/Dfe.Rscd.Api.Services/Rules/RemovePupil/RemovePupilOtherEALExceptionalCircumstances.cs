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
        private readonly IAllocationYearConfig _config;
        
        private const string ReasonDescription = "Other - EAL exceptional circumstances";
        private string _evidenceHelpDeskText => Content.RemovePupilOtherEalExceptionalCirumstances_HTML;

        public RemovePupilOtherEalExceptionalCircumstances(IDataService dataService, IAllocationYearConfig config)
        {
            _dataService = dataService;
            _config = config;
        }
        public override AmendmentType AmendmentType => AmendmentType.RemovePupil;
        public override int AmendmentReason => (int) AmendmentReasonCode.OtherEAL;
        
        public override List<Question> GetQuestions(Amendment amendment)
        {
            var questions = new List<Question>();
            var countries = _dataService.GetAnswerPotentials(nameof(PupilCountryQuestion));
            var languages = _dataService.GetAnswerPotentials(nameof(PupilNativeLanguageQuestion));

            var nativeLanguageQuestion = new PupilNativeLanguageQuestion(languages.ToList());
            questions.Add(nativeLanguageQuestion);
            
            var countryQuestion = new PupilCountryQuestion(countries.ToList());
            questions.Add(countryQuestion);
            
            var pupilArrivalToUk = new ArrivalDateQuestion();
            questions.Add(pupilArrivalToUk);
            
            var evidenceQuestion = new EvidenceUploadQuestion(_evidenceHelpDeskText);
            questions.Add(evidenceQuestion);

            return questions;
        }
        

        protected override List<ValidatedAnswer> GetValidatedAnswers(Amendment amendment)
        {
            var answers = new List<ValidatedAnswer>();
            var questions = GetQuestions(amendment);
            
            var languageAnswer = questions.Single(x => x.Id == nameof(PupilNativeLanguageQuestion));
            var countryAnswer = questions.Single(x => x.Id == nameof(PupilCountryQuestion));
            var studentArrivalDate = questions.Single(x => x.Id == nameof(ArrivalDateQuestion));
            var evidenceAnswer = questions.Single(x => x.Id == nameof(EvidenceUploadQuestion));
            return new List<ValidatedAnswer>
            {
                languageAnswer.GetAnswer(amendment),
                countryAnswer.GetAnswer(amendment),
                studentArrivalDate.GetAnswer(amendment),
                evidenceAnswer.GetAnswer(amendment)
            };
        }

        protected override AmendmentOutcome ApplyRule(Amendment amendment, List<ValidatedAnswer> answers)
        {
            var arrivalDate = answers.Single(x => x.QuestionId == nameof(ArrivalDateQuestion));
            var evidenceUploadQuestion = answers.Single(x => x.QuestionId == nameof(EvidenceUploadQuestion));

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

        protected override void ApplyOutcomeToAmendment(Amendment amendment, AmendmentOutcome amendmentOutcome, List<ValidatedAnswer> answers)
        {
            if (amendmentOutcome.IsComplete && amendmentOutcome.FurtherQuestions == null)
            {
                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_ReasonDescription,
                    amendmentOutcome.ReasonDescription);

                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_ReasonCode,
                    amendmentOutcome.ReasonId);
                
                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_SubReasonDescription,
                    amendmentOutcome.SubReason);

                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_OutcomeDescription,
                    amendmentOutcome.OutcomeDescription);
                
                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_DateOfArrivalUk,
                    GetAnswer(answers, nameof(ArrivalDateQuestion)).Value);
                
                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_CountryOfOrigin, 
                    GetAnswer(answers, nameof(PupilCountryQuestion)).Value);
                
                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_NativeLanguage, 
                    GetAnswer(answers, nameof(PupilNativeLanguageQuestion)).Value);
            }
        }
    }
}