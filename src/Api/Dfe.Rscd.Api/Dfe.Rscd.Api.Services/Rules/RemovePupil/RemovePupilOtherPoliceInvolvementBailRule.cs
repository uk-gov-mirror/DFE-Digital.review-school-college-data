using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class RemovePupilOtherPoliceInvolvementBailRule: RemovePupilOtherEvidenceOnlyRule
    {
        public override int AmendmentReason => (int) AmendmentReasonCode.OtherPoliceInvolvementBailRestrictions;
        protected override string ReasonDescription => "Other - Police involvement/bail restrictions";

        protected override string EvidenceHelperTextHtml =>
            "<p>Evidence to provide:</p><ul><li>Date of incident</li><li>Crime reference number</li><li>Police report</li><li>Restrictions put in place which prevented pupil accessing education</li><li>Alternative education provided</li><ul/>";
    }
}