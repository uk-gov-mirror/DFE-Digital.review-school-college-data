using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class RemovePupilOtherTerminalLongIllnessRule: RemovePupilOtherEvidenceOnlyRule
    {
        public override int AmendmentReason => (int) AmendmentReasonCode.OtherTerminalIllness;
        protected override string ReasonDescription => "Other - Terminal/Long illness";

        protected override string EvidenceHelperTextHtml =>
            "<p>Evidence to provide:</p><ul><li>Recent supporting statement from a medical professional stating pupil unable to attend school/sit exams and medical evidence to support long term illness</li><li>% Attendance - Yr 10</li><li>% Attendance - Yr 11</li><li>Details of support provided to pupil by school</li><li>Details of home or hospital tuition: how many weeks/hours per week</li><li>Name of the hospital where the pupil was an inpatient and unable to study</li><li>Dates when the pupil was an inpatient, or held in a secure hospital under the Mental Health Act</li><ul/>";
    }
}