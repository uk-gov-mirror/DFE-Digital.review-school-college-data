using System.Linq;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Interfaces;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class RemovePupilRules : IRuleSet
    {
        public OutcomeStatus Apply(Amendment amendment)
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
                        return OutcomeStatus.AutoAccept;

                    return OutcomeStatus.AutoReject;
                }

                case 326: // International student
                    if (isAoAllocated &&
                        amendment.Pupil.Allocations.All(a =>
                            a.Allocation == Allocation.Unknown || a.Allocation == Allocation.NotAllocated ||
                            a.Allocation == Allocation.AwardingOrganisation))
                        return OutcomeStatus.AutoAccept;
                    return OutcomeStatus.AutoReject;

                case 327: // Deceased
                    return wasAllocatedAny ? OutcomeStatus.AutoAccept : OutcomeStatus.AutoReject;

                case 328: // Not on roll
                    if (isAoAllocated)
                    {
                        amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_ScrutinyReasonCode,ScrutinyReason.NotOnRoll);
                        amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_AmdFlag, "NR");

                        return OutcomeStatus.AutoAccept;
                    }

                    amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_ScrutinyReasonCode, ScrutinyReason.NotOnRoll);
                    return OutcomeStatus.AutoReject;

                case 330: // Evidence not required
                    if (!isAoAllocated)
                    {
                        amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_ScrutinyReasonCode, ScrutinyReason.OtherWithoutEvidence);
                        amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_AmdFlag, "NR");

                        return OutcomeStatus.AutoReject;
                    }

                    amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_ScrutinyReasonCode, ScrutinyReason.OtherWithoutEvidence);
                    amendment.AmendmentDetail.SetField(RemovePupilAmendment.FIELD_AmdFlag, "NR");

                    return OutcomeStatus.AwatingDfeReview;

                default:
                    return OutcomeStatus.AwatingDfeReview;
            }
        }

        public AmendmentType AmendmentType => AmendmentType.RemovePupil;
    }
}