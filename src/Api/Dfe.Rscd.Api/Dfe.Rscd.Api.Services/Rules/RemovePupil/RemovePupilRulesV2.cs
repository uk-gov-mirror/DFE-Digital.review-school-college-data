using System;
using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.BusinessLogic.Entities;
using Dfe.Rscd.Api.Infrastructure.SqlServer.Repositories;

namespace Dfe.Rscd.Api.Services.Rules
{
    public partial class RemovePupilRulesV2 : BaseRules, IRuleSet
    {
        public RemovePupilRulesV2(IDataRepository dataRepository) : base(dataRepository){ }

        public override AmendmentType AmendmentType => AmendmentType.RemovePupil;


        public override AdjustmentOutcome Apply(Amendment amendment)
        {
            AdjustmentOutcome adjOutcomeOut;

            var student = amendment.Pupil;

            switch (amendment.InclusionReasonId)
            {
                //Reason 8
                case ((int)ReasonsForAdjustment.AdmittedFromAbroad):
                    adjOutcomeOut =
                        AdmittedFromAbroad(amendment.DfesNumber, student, amendment.InclusionReasonId, amendment.Answers);
                    break;

                //Reason 10
                case ((int)ReasonsForAdjustment.AdmittedFollowingPermanentExclusionFromMaintainedSchool):
                    adjOutcomeOut =
                        AdmittedFollowingPermanentExclusion(student, amendment.InclusionReasonId, amendment.Answers);
                    break;

                //Reason 12
                case ((int)ReasonsForAdjustment.Deceased):
                    adjOutcomeOut =
                        ProcessInclusionPromptResponses_Deceased(amendment.InclusionReasonId, amendment.Answers, student.Id);
                    break;

                //Reason 19
                case ((int)ReasonsForAdjustment.KS4Other):
                    adjOutcomeOut =
                        ProcessInclusionPromptResponses_OtherKS4(student, amendment.InclusionReasonId, amendment.Answers);
                    break;

                //Reason 54
                case ((int)ReasonsForAdjustment.NotAtEndOfAdvancedStudy):
                    adjOutcomeOut =
                        ProcessInclusionPromptResponses_NotAtEndOfAdvancedStudy(student, amendment.InclusionReasonId, amendment.Answers);
                    break;

                default:
                    adjOutcomeOut = new AdjustmentOutcome(new CompletedNonStudentAdjustment("Adjustment reason not found"));
                    break;
            }

            ApplyOutcomeToAmendment(amendment, adjOutcomeOut);

            return adjOutcomeOut;
        }

        public void ApplyOutcomeToAmendment(Amendment amendment, AdjustmentOutcome adjustmentOutcome)
        {
            if (adjustmentOutcome.IsComplete && adjustmentOutcome.FurtherPrompts.Count == 0 && adjustmentOutcome.CompletedRequest != null)
            {
                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_ScrutinyReasonCode, adjustmentOutcome.CompletedRequest.ScrutinyStatusCode);
                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_ReasonCode, adjustmentOutcome.CompletedRequest.InclusionReasonID);
                amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_Detail, adjustmentOutcome.CompletedRequest.ScrutinyStatusDescription);
            }
        }
    }
}