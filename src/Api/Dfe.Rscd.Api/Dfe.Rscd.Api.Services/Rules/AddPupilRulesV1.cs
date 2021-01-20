using Dfe.Rscd.Api.BusinessLogic.Entities;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class AddPupilRulesV1 : IRuleSet
    {
        public AmendmentOutcome Apply(Amendment amendment)
        {
            return new AmendmentOutcome(new CompleteSimpleOutcomeCheck(OutcomeStatus.AwatingDfeReview));
        }

        public AmendmentType AmendmentType => AmendmentType.AddPupil;
    }
}