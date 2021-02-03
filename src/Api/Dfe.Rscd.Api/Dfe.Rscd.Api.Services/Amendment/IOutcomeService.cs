using Dfe.CspdAlpha.Web.Infrastructure.Crm;
using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Services
{
    public interface IOutcomeService
    {
        AmendmentOutcome ApplyRules(rscd_Amendment amendmentDto, Amendment amendment);
    }
}