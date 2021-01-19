using System.Linq;
using Dfe.Rscd.Api.BusinessLogic.Entities;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class RemovePupilRulesV1 : IRuleSet
    {
        public AdjustmentOutcome Apply(Amendment amendment)
        {
            var inCurrentAllocationYear = amendment.Pupil.Allocations != null &&
                                          amendment.Pupil.Allocations.Any() &&
                                          amendment.Pupil.Allocations.First().Allocation !=
                                          Allocation.NotAllocated;

            var wasAllocatedAny = amendment.Pupil.Allocations != null &&
                                  amendment.Pupil.Allocations.Any() &&
                                  amendment.Pupil.Allocations.FirstOrDefault(a =>
                                      a.Allocation != Allocation.Unknown && a.Allocation != Allocation.NotAllocated) !=
                                  null;

            var isAoAllocated = amendment.Pupil.Allocations != null &&
                                amendment.Pupil.Allocations.Any(a =>
                                    a.Allocation == Allocation.AwardingOrganisation);

            switch (amendment.AmendmentDetail.GetField<int?>(RemovePupilAmendment.FIELD_ReasonCode))
            {
                case 325: // Not at the end of 16-18 study
                {
                    if (amendment.Pupil.Age < 18 && inCurrentAllocationYear)
                        return new AdjustmentOutcome(new CompleteSimpleOutcomeCheck(OutcomeStatus.AutoAccept));

                    return new AdjustmentOutcome(new CompleteSimpleOutcomeCheck(OutcomeStatus.AutoReject));
                }

                case 326: // International student
                    if (isAoAllocated &&
                        amendment.Pupil.Allocations.All(a =>
                            a.Allocation == Allocation.Unknown || a.Allocation == Allocation.NotAllocated ||
                            a.Allocation == Allocation.AwardingOrganisation))
                        return new AdjustmentOutcome(new CompleteSimpleOutcomeCheck(OutcomeStatus.AutoAccept));
                    return new AdjustmentOutcome(new CompleteSimpleOutcomeCheck(OutcomeStatus.AutoReject));

                case 327: // Deceased
                    return wasAllocatedAny ? new AdjustmentOutcome(new CompleteSimpleOutcomeCheck(OutcomeStatus.AutoAccept)) : 
                        new AdjustmentOutcome(new CompleteSimpleOutcomeCheck(OutcomeStatus.AutoReject));

                case 328: // Not on roll
                    if (!wasAllocatedAny)
                    {
                        throw new NotAllowedException("Not on roll is not a valid reason", "Insufficient information provided as answer to student adjustment prompts.");
                    }
                    if (isAoAllocated)
                    {
                        amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_ScrutinyReasonCode,ScrutinyReason.NotOnRoll);
                        amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_AmdFlag, "NR");

                        return new AdjustmentOutcome(new CompleteSimpleOutcomeCheck(OutcomeStatus.AutoAccept));
                    }

                    throw new NotAllowedException("Not on roll is not a valid reason", "You cannot select 'Not on roll' as you are not the core provider for the attendance year or School Census/ILR data confirms the student is on roll.");

                case 330: // Evidence not required
                    if (!isAoAllocated)
                    {
                        amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_ScrutinyReasonCode, ScrutinyReason.OtherWithoutEvidence);
                        amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_AmdFlag, "NR");

                        return new AdjustmentOutcome(new CompleteSimpleOutcomeCheck(OutcomeStatus.AutoReject));
                    }

                    amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_ScrutinyReasonCode, ScrutinyReason.OtherWithoutEvidence);
                    amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_AmdFlag, "NR");

                    return new AdjustmentOutcome(new CompleteSimpleOutcomeCheck(OutcomeStatus.AwatingDfeReview));

                default:
                    return new AdjustmentOutcome(new CompleteSimpleOutcomeCheck(OutcomeStatus.AwatingDfeReview));
            }
        }

        public AmendmentType AmendmentType => AmendmentType.RemovePupil;
    }
}