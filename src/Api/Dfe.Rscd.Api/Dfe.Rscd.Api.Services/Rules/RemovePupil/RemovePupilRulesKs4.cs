using Dfe.Rscd.Api.BusinessLogic.Entities;
using Dfe.Rscd.Api.Infrastructure.SqlServer.Repositories;

namespace Dfe.Rscd.Api.Services.Rules
{
    public partial class RemovePupilRulesKs4 : BaseRules
    {
        public RemovePupilRulesKs4(IDataRepository dataRepository) : base(dataRepository)
        {
        }

        public override AmendmentType AmendmentType => AmendmentType.RemovePupil;
        public override CheckingWindow CheckingWindow => CheckingWindow.KS4June;


        public override AmendmentOutcome Apply(Amendment amendment)
        {
            AmendmentOutcome adjOutcomeOut;

            var student = amendment.Pupil;

            switch (amendment.InclusionReasonId)
            {
                //Reason 8
                case (int) ReasonsForAdjustment.AdmittedFromAbroad:
                    adjOutcomeOut =
                        AdmittedFromAbroad(amendment.DfesNumber, student, amendment.InclusionReasonId,
                            amendment.Answers);
                    break;

                //Reason 10
                case (int) ReasonsForAdjustment.AdmittedFollowingPermanentExclusionFromMaintainedSchool:
                    adjOutcomeOut =
                        AdmittedFollowingPermanentExclusion(student, amendment.InclusionReasonId, amendment.Answers);
                    break;

                //Reason 12
                case (int) ReasonsForAdjustment.Deceased:
                    adjOutcomeOut =
                        ProcessInclusionPromptResponses_Deceased(amendment.InclusionReasonId, amendment.Answers,
                            student.Id);
                    break;

                //Reason 19
                case (int) ReasonsForAdjustment.KS4Other:
                    adjOutcomeOut =
                        ProcessInclusionPromptResponses_OtherKS4(student, amendment.InclusionReasonId,
                            amendment.Answers);
                    break;

                //Reason 54
                case (int) ReasonsForAdjustment.NotAtEndOfAdvancedStudy:
                    adjOutcomeOut =
                        ProcessInclusionPromptResponses_NotAtEndOfAdvancedStudy(student, amendment.InclusionReasonId,
                            amendment.Answers);
                    break;

                default:
                    adjOutcomeOut =
                        new AmendmentOutcome(new CompletedNonStudentAdjustment("Adjustment reason not found"));
                    break;
            }

            ApplyOutcomeToAmendment(amendment, adjOutcomeOut);

            return adjOutcomeOut;
        }

        public void ApplyOutcomeToAmendment(Amendment amendment, AmendmentOutcome amendmentOutcome)
        {
            if (amendmentOutcome.IsComplete && amendmentOutcome.FurtherPrompts == null &&
                amendmentOutcome.CompletedRequest != null)
            {
                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_ScrutinyReasonCode,
                    amendmentOutcome.CompletedRequest.ScrutinyStatusCode);
                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_ReasonCode,
                    amendmentOutcome.CompletedRequest.InclusionReasonID);
                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_Detail,
                    amendmentOutcome.CompletedRequest.ScrutinyStatusDescription);
            }
        }
    }
}