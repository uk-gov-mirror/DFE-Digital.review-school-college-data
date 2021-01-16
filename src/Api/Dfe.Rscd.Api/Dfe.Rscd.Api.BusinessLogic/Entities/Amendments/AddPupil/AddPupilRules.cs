namespace Dfe.Rscd.Api.BusinessLogic.Entities
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