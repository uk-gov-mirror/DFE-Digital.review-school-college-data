using Dfe.CspdAlpha.Web.Infrastructure.Crm;
using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Interfaces
{
    public interface IOutcomeService
    {
        void SetOutcome(rscd_Amendment amendmentDto, Amendment amendment);
    }
}