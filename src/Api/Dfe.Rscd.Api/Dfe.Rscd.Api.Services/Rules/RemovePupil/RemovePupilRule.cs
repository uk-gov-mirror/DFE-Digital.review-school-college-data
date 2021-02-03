using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Services.Rules
{
    public abstract class RemovePupilRule : IRule
    {
        public AmendmentType AmendmentType => AmendmentType.RemovePupil;
        public abstract int AmendmentReason { get; }

        protected abstract AmendmentOutcome ApplyRule(Amendment amendment);

        public AmendmentOutcome Apply(Amendment amendment)
        {
            AmendmentOutcome amendmentOutcome = ApplyRule(amendment);
            
            ApplyOutcomeToAmendment(amendment, amendmentOutcome);

            return amendmentOutcome;
        }

        protected void ApplyOutcomeToAmendment(Amendment amendment, AmendmentOutcome amendmentOutcome)
        {
            if (amendmentOutcome.IsComplete && amendmentOutcome.FurtherQuestions == null)
            {
                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_ScrutinyReasonCode,
                    amendmentOutcome.ScrutinyStatusCode);
                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_ReasonCode,
                    amendmentOutcome.ReasonId);
                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_Detail,
                    amendmentOutcome.ScrutinyDetail);
            }
        }
    }
}