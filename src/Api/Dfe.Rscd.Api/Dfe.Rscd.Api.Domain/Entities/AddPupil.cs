using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Interfaces;
using System.Collections.Generic;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class AddPupil : IAmendmentType
    {
        public AddReason Reason { get; set; }
        public string PreviousSchoolURN { get; set; }
        public string PreviousSchoolLAEstab { get; set; }
        public List<PriorAttainment> PriorAttainmentResults { get; set; }

        public OutcomeStatus GetOutcomeStatus(Amendment amendment)
        {
            return OutcomeStatus.AwatingDfeReview;
        }
    }
}
