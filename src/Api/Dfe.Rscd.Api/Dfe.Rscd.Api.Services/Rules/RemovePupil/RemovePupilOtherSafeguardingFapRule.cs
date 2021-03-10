using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class RemovePupilOtherSafeguardingFapRule: RemovePupilOtherEvidenceOnlyRule
    {
        public override int AmendmentReason => (int) AmendmentReasonCode.OtherSafeguardingFAP;
        protected override string ReasonDescription => "Other - Safeguarding/FAP";

        protected override string EvidenceHelperTextHtml => Content.RemovePupilOtherSafeguardingFapRule_HTML;
    }
}