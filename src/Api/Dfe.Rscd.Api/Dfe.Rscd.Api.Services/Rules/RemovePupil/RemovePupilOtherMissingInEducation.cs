using System.Collections.Generic;
using Dfe.Rscd.Api.Domain.Common;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Entities.Amendments;
using Dfe.Rscd.Api.Domain.Entities.Questions;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Config;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class RemovePupilOtherMissingInEducation : Rule
    {
        private readonly IAllocationYearConfig _config;

        public RemovePupilOtherMissingInEducation(IAllocationYearConfig config)
        {
            _config = config;
        }
        
        public override AmendmentType AmendmentType => AmendmentType.RemovePupil;
        public override List<Question> GetQuestions(Amendment amendment)
        {
            var questions = new List<Question>();

            var pupilDateOffRoleQuestion = new PupilDateOffRollQuestion();
            questions.Add(pupilDateOffRoleQuestion);

            var explainQuestion = new ExplainYourRequestQuestion(null);
            questions.Add(explainQuestion);
            
            var evidenceQuestion = new EvidenceUploadQuestion(Content.RemovePupilOtherPermanentlyExcluded_HTML);
            questions.Add(evidenceQuestion);

            return questions;
        }

        protected override AmendmentOutcome ApplyRule(Amendment amendment)
        {
            var dateOffRoll = GetAnswer(amendment, nameof(PupilDateOffRollQuestion));
            var evidenceUploadQuestion = GetAnswer(amendment, nameof(EvidenceUploadQuestion));

            amendment.EvidenceStatus = string.IsNullOrEmpty(evidenceUploadQuestion.Value) || evidenceUploadQuestion.Value == "0"
                ? EvidenceStatus.Later : EvidenceStatus.Now;

            return new AmendmentOutcome(OutcomeStatus.AwatingDfeReview, null)
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
                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_ReasonDescription,
                    amendmentOutcome.ReasonDescription);

                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_ReasonCode,
                    amendmentOutcome.ReasonId);
                
                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_SubReasonDescription,
                    amendmentOutcome.SubReason);

                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_OutcomeDescription,
                    amendmentOutcome.OutcomeDescription);
                
                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_DateOffRoll,
                    GetAnswer(amendment, nameof(PupilDateOffRollQuestion)).Value);
            }
        }

        public override int AmendmentReason => (int) AmendmentReasonCode.OtherPupilMissingInEducation;
        public string ReasonDescription => "Other - Pupil missing in education";
    }
}