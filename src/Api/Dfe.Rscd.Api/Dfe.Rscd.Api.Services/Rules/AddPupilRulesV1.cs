using Dfe.Rscd.Api.BusinessLogic.Entities;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class AddPupilRulesV1 : IRuleSet
    {
        public AdjustmentOutcome Apply(Amendment amendment)
        {
            return new AdjustmentOutcome(new CompleteSimpleOutcomeCheck(OutcomeStatus.AwatingDfeReview));
        }

        public AmendmentType AmendmentType => AmendmentType.AddPupil;
    }
}