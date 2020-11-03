using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Interfaces;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class RemovePupil : IAmendmentType
    {
        public string  Reason { get; set; }
        public string SubReason { get; set; }
        public string Detail { get; set; }
        public string AllocationYear { get; set; }

        public OutcomeStatus GetOutcomeStatus(Amendment amendment)
        {
            switch (Reason)
            {
                case "325": // Not a the end of 16-18 study
                {
                    if (amendment.Pupil.Age < 18)  // TODO: source of allocation logic required herepo
                    {
                        return OutcomeStatus.AutoAccept;
                    }

                    return OutcomeStatus.AutoReject;
                }
                case "327": // Deceased
                    // TODO: ensure latest core providerc
                    return OutcomeStatus.AutoAccept;
                case "328": // Not on roll
                    // TODO: source of allocation logic if AO then accept
                    return OutcomeStatus.AutoReject;
                case "330": // Evidence not required
                    return OutcomeStatus.AutoReject;
                default:
                    return OutcomeStatus.AwatingDfeReview;
            }
        }
    }
}
