using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Services.Rules
{
    public abstract class RemovePupilRule : Rule
    {
        public override AmendmentType AmendmentType => AmendmentType.RemovePupil;
        
        protected override void ApplyOutcomeToAmendment(Amendment amendment, AmendmentOutcome amendmentOutcome)
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