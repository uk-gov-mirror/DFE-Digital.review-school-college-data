using Dfe.Rscd.Api.Domain.Core.Enums;

namespace Dfe.Rscd.Api.Domain.Interfaces
{
    public interface IRuleSet
    {
        OutcomeStatus Apply(IAmendment amendment);

        AmendmentType AmendmentType { get; }
    }
}