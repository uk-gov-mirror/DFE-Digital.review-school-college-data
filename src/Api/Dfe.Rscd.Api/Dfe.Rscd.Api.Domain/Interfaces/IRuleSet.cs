using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Domain.Interfaces
{
    public interface IRuleSet
    {
        AmendmentType AmendmentType { get; }
        OutcomeStatus Apply(Amendment amendment);
    }
}