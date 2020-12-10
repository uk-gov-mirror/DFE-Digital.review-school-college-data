using System.Linq;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Interfaces;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class RemovePupil : IAmendmentType
    {
        public int ReasonCode { get; set; }

        public string SubReason { get; set; }

        public string Detail { get; set; }

        public int[] AllocationYears { get; set; }

        public OutcomeStatus GetOutcomeStatus(Amendment amendment)
        {
            switch (ReasonCode)
            {
                case 325: // Not at the end of 16-18 study
                {
                    if (amendment.Pupil.Age < 18 && amendment.Pupil.InCurrentAllocationYear)
                    {
                        return OutcomeStatus.AutoAccept;
                    }

                    return OutcomeStatus.AutoReject;
                }

                case 326: // International student
                    if (amendment.Pupil.Allocations.Any(a => a.Allocation == Allocation.AwardingOrganisation) &&
                        amendment.Pupil.Allocations.All(a => a.Allocation == Allocation.Unknown || a.Allocation == Allocation.NotAllocated || a.Allocation == Allocation.AwardingOrganisation))
                    {
                        return OutcomeStatus.AutoAccept;
                    }
                    return OutcomeStatus.AutoReject;

                case 327: // Deceased
                    return amendment.Pupil.WasAllocatedAny ? OutcomeStatus.AutoAccept : OutcomeStatus.AutoReject;

                case 328: // Not on roll
                    if (amendment.Pupil.IsAOAllocated(AllocationYears))
                    {
                        amendment.ScrutinyReasonCode = ScrutinyReason.NotOnRoll;
                        amendment.AmdFlag = "NR";

                        return OutcomeStatus.AutoAccept;
                    }

                    amendment.ScrutinyReasonCode = ScrutinyReason.NotOnRoll;
                    return OutcomeStatus.AutoReject;

                case 330: // Evidence not required
                    if(!amendment.Pupil.IsAOAllocated(AllocationYears))
                    {
                        amendment.ScrutinyReasonCode = ScrutinyReason.OtherWithoutEvidence;
                        amendment.AmdFlag = "NR";

                        return OutcomeStatus.AutoReject;
                    }

                    amendment.ScrutinyReasonCode = 102;

                    return OutcomeStatus.AwatingDfeReview;

                default:
                    return OutcomeStatus.AwatingDfeReview;
            }
        }
    }
}
