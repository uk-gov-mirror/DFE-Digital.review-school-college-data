using Dfe.Rscd.Api.BusinessLogic.Contracts.Entities.Amendments;
using Dfe.Rscd.Api.BusinessLogic.Contracts.Entities.Core.Enums;

namespace Dfe.Rscd.Api.BusinessLogic.Contracts.Entities
{
    public interface IRuleSet
    {
        AmendmentType AmendmentType { get; }
        OutcomeStatus Apply(Amendment amendment);
    }
}