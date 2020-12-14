using System.Linq;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Interfaces;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class RemovePupilRules : IRuleSet
    {
        public OutcomeStatus Apply(Amendment amendment)
        {
            var removePupilAmendment = (RemovePupilAmendment) amendment;

            var inCurrentAllocationYear = removePupilAmendment.Pupil.Allocations != null &&
                                          removePupilAmendment.Pupil.Allocations.Any() &&
                                          removePupilAmendment.Pupil.Allocations.First().Allocation !=
                                          Allocation.NotAllocated;

            var wasAllocatedAny = removePupilAmendment.Pupil.Allocations != null &&
                                  removePupilAmendment.Pupil.Allocations.Any() &&
                                  removePupilAmendment.Pupil.Allocations.FirstOrDefault(a =>
                                      a.Allocation != Allocation.Unknown && a.Allocation != Allocation.NotAllocated) !=
                                  null;

            var isAoAllocated = removePupilAmendment.Pupil.Allocations != null &&
                                removePupilAmendment.Pupil.Allocations.Any(a =>
                                    a.Allocation == Allocation.AwardingOrganisation);

            switch (removePupilAmendment.AmendmentDetail.GetField<int?>("ReasonCode"))
            {
                case 325: // Not at the end of 16-18 study
                {
                    if (removePupilAmendment.Pupil.Age < 18 && inCurrentAllocationYear)
                        return OutcomeStatus.AutoAccept;

                    return OutcomeStatus.AutoReject;
                }

                case 326: // International student
                    if (isAoAllocated &&
                        removePupilAmendment.Pupil.Allocations.All(a =>
                            a.Allocation == Allocation.Unknown || a.Allocation == Allocation.NotAllocated ||
                            a.Allocation == Allocation.AwardingOrganisation))
                        return OutcomeStatus.AutoAccept;
                    return OutcomeStatus.AutoReject;

                case 327: // Deceased
                    return wasAllocatedAny ? OutcomeStatus.AutoAccept : OutcomeStatus.AutoReject;

                case 328: // Not on roll
                    if (isAoAllocated)
                    {
                        removePupilAmendment.ScrutinyReasonCode = ScrutinyReason.NotOnRoll;
                        removePupilAmendment.AmdFlag = "NR";

                        return OutcomeStatus.AutoAccept;
                    }

                    removePupilAmendment.ScrutinyReasonCode = ScrutinyReason.NotOnRoll;
                    return OutcomeStatus.AutoReject;

                case 330: // Evidence not required
                    if (!isAoAllocated)
                    {
                        removePupilAmendment.ScrutinyReasonCode = ScrutinyReason.OtherWithoutEvidence;
                        removePupilAmendment.AmdFlag = "NR";

                        return OutcomeStatus.AutoReject;
                    }

                    removePupilAmendment.ScrutinyReasonCode = 102;

                    return OutcomeStatus.AwatingDfeReview;

                default:
                    return OutcomeStatus.AwatingDfeReview;
            }
        }

        public AmendmentType AmendmentType => AmendmentType.RemovePupil;
    }
}