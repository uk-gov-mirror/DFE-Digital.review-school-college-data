using Dfe.Rscd.Api.BusinessLogic.Entities;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class AddPupilRulesKs4 : IRuleSet
    {
        public AmendmentOutcome Apply(Amendment amendment)
        {
            return new AmendmentOutcome(new CompleteSimpleOutcomeCheck(OutcomeStatus.AwatingDfeReview));
        }

        public CheckingWindow CheckingWindow => CheckingWindow.KS4June;

        public AmendmentType AmendmentType => AmendmentType.AddPupil;
    }
}