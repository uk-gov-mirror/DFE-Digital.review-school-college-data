using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Interfaces;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class AddPupilRules : IRuleSet
    {
        public OutcomeStatus Apply(Amendment amendment)
        {
            return OutcomeStatus.AwatingDfeReview;
        }

        public AmendmentType AmendmentType => AmendmentType.AddPupil;
    }
}