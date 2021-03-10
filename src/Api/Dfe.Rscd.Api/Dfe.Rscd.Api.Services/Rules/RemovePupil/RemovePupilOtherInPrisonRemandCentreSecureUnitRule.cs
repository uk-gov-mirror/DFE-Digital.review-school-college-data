using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class RemovePupilOtherInPrisonRemandCentreSecureUnitRule : RemovePupilOtherEvidenceOnlyRule
    {
        public override int AmendmentReason => (int) AmendmentReasonCode.OtherPrisonRemandCentreSecureUnit;
        protected override string ReasonDescription => "Other - In prison/remand centre/secure unit";

        protected override string EvidenceHelperTextHtml => Content.RemovePupilOtherInPrisonRemandCentreSecureUnitRule_HTML;
    }
}