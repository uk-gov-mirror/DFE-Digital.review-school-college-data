using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class RemovePupilOtherInPrisonRemandCentreSecureUnitRule : RemovePupilOtherEvidenceOnlyRule
    {
        public override int AmendmentReason => (int) AmendmentReasonCode.OtherPrisonRemandCentreSecureUnit;
        protected override string ReasonDescription => "Other - In prison/remand centre/secure unit";

        protected override string EvidenceHelperTextHtml =>
            "<p>Evidence to provide:</p><ul><li>Dates spent in prison/ remand centre/secure unit</li><li>Details of prison/ remand centre/secure unit</li><ul/>";
    }
}