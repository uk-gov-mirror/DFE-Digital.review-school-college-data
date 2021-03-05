using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class RemovePupilOtherSafeguardingFapRule: RemovePupilOtherEvidenceOnlyRule
    {
        public override int AmendmentReason => (int) AmendmentReasonCode.OtherSafeguardingFAP;
        protected override string ReasonDescription => "Other - Safeguarding/FAP";

        protected override string EvidenceHelperTextHtml =>
            "<p>Evidence to provide:</p><ul><li>Evidence from the governing body, MAT or local authority giving the timing of the move and the reason why the pupil was placed in your school</li><ul/>";
    }
}