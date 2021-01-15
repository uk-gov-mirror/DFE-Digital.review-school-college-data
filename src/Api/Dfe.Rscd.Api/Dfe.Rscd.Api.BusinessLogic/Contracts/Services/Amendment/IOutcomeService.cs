using Dfe.CspdAlpha.Web.Infrastructure.Crm;
using Dfe.Rscd.Api.BusinessLogic.Contracts.Entities.Amendments;

namespace Dfe.Rscd.Api.BusinessLogic.Contracts.Services
{
    public interface IOutcomeService
    {
        void SetOutcome(rscd_Amendment amendmentDto, Amendment amendment);
    }
}