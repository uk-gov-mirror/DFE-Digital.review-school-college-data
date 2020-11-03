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
                case "325":
                {
                    if (amendment.Pupil.Age < 18)  // TODO: source of allocation logic required herepo
                    {
                        return OutcomeStatus.AutoAccept;
                    }

                    return OutcomeStatus.AutoReject;
                }
                case "330":
                    return OutcomeStatus.AutoReject;
                default:
                    return OutcomeStatus.AwatingDfeReview;
            }
        }
    }
}
