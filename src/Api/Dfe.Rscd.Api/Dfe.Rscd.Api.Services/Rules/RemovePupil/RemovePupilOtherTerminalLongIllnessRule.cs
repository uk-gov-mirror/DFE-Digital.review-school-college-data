using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class RemovePupilOtherTerminalLongIllnessRule: RemovePupilOtherEvidenceOnlyRule
    {
        public override int AmendmentReason => (int) AmendmentReasonCode.OtherTerminalIllness;
        protected override string ReasonDescription => "Other - Terminal/Long illness";

        protected override string EvidenceHelperTextHtml => Content.RemovePupilOtherTerminalLongIllnessRule_HTML;
    }
}