using Dfe.CspdAlpha.Web.Infrastructure.Crm;
using Dfe.Rscd.Api.BusinessLogic.Entities;

namespace Dfe.Rscd.Api.Services
{
    public interface IOutcomeService
    {
        AdjustmentOutcome ApplyRules(rscd_Amendment amendmentDto, Amendment amendment);
    }
}