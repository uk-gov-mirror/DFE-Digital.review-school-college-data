using Dfe.Rscd.Api.BusinessLogic.Entities;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class AddPupilRulesV1 : IRuleSet
    {
        public AdjustmentOutcome Apply(Amendment amendment)
        {
            return new AdjustmentOutcome(new CompleteSimpleOutcomeCheck(OutcomeStatus.AwatingDfeReview));
        }

        public RuleSetContext Context { get; set; }

        public AmendmentType AmendmentType => AmendmentType.AddPupil;
    }
}