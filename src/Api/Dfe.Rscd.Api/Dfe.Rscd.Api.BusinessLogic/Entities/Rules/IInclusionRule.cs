namespace Dfe.Rscd.Api.Domain.Entities
{
    public interface IInclusionRule
    {
        AmendmentOutcome Apply(Amendment amendment);
        int AmendmentReason { get; }
    }
}