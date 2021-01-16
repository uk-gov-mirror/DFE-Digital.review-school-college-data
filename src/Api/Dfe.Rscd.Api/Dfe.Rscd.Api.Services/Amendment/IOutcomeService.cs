using Dfe.CspdAlpha.Web.Infrastructure.Crm;
using Dfe.Rscd.Api.BusinessLogic.Entities;

namespace Dfe.Rscd.Api.Services
{
    public interface IOutcomeService
    {
        void SetOutcome(rscd_Amendment amendmentDto, Amendment amendment);
    }
}