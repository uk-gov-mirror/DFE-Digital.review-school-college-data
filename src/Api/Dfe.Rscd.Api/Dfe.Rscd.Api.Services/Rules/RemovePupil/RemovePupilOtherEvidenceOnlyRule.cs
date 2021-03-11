using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Entities.Amendments;
using Dfe.Rscd.Api.Domain.Entities.Questions;

namespace Dfe.Rscd.Api.Services.Rules
{
    public abstract class RemovePupilOtherEvidenceOnlyRule : Rule
    {
        public override AmendmentType AmendmentType => AmendmentType.RemovePupil;
        public override List<Question> GetQuestions(Amendment amendment)
        {
            var evidenceQuestion = new EvidenceUploadQuestion(EvidenceHelperTextHtml);
            return new List<Question> { evidenceQuestion };
        }

        protected override List<ValidatedAnswer> GetValidatedAnswers(Amendment amendment)
        {
            var questions = GetQuestions(amendment);
            var evidenceAnswer = questions.Single(x => x.Id == nameof(EvidenceUploadQuestion));
            return new List<ValidatedAnswer> { evidenceAnswer.GetAnswer(amendment) };
        }

        protected override AmendmentOutcome ApplyRule(Amendment amendment, List<ValidatedAnswer> answers)
        {
            var evidenceUploadQuestion = answers.Single(x => x.QuestionId == nameof(EvidenceUploadQuestion));

            amendment.EvidenceStatus = string.IsNullOrEmpty(evidenceUploadQuestion.Value) || evidenceUploadQuestion.Value == "0"
                ? EvidenceStatus.Later : EvidenceStatus.Now;
            
            return new AmendmentOutcome(OutcomeStatus.AwatingDfeReview, ReasonDescription)
            {
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
            }
        }
        
        protected abstract string ReasonDescription { get; }
        protected abstract string EvidenceHelperTextHtml { get; }
    }
}