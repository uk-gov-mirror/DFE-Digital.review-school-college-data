namespace Dfe.Rscd.Api.BusinessLogic.Entities
{
    public interface IRuleSet
    {
        AmendmentType AmendmentType { get; }
        OutcomeStatus Apply(Amendment amendment);
    }
}