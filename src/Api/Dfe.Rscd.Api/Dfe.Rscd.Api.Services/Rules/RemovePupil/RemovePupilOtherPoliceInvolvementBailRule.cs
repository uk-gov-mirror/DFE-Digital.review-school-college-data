using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class RemovePupilOtherPoliceInvolvementBailRule: RemovePupilOtherEvidenceOnlyRule
    {
        public override int AmendmentReason => (int) AmendmentReasonCode.OtherPoliceInvolvementBailRestrictions;
        protected override string ReasonDescription => "Other - Police involvement/bail restrictions";

        protected override string EvidenceHelperTextHtml => Content.RemovePupilOtherPoliceInvolvementBailRule_HTML;
    }
}