using Dfe.Rscd.Api.BusinessLogic.Contracts.Entities.Core.Enums;

namespace Dfe.Rscd.Api.BusinessLogic.Contracts.Entities.Amendments.AddPupil
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