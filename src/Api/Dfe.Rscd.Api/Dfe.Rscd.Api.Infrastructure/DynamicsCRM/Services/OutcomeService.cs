using Dfe.CspdAlpha.Web.Infrastructure.Crm;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Interfaces;

namespace Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Services
{
    public class OutcomeService : IOutcomeService
    {
        public void SetOutcome(rscd_Amendment amendmentDto, Amendment amendment)
        {
            if (amendment.EvidenceStatus == EvidenceStatus.Later)
            {
                amendmentDto.rscd_Outcome = rscd_Outcome.Awaitingevidence;
            }
            else if (amendment.AmendmentType == AmendmentType.RemovePupil && ((RemovePupil)amendment.AmendmentDetail).Reason == "330") // Other - evidence not required
            {
                amendmentDto.rscd_Outcome = rscd_Outcome.Autorejected;
                amendmentDto.rscd_Amendmentstatus = new_amendmentstatus.Rejected;
                //amendmentDto.StateCode = rscd_AmendmentState.Inactive;
            }
            else
            {
                amendmentDto.rscd_Outcome = rscd_Outcome.AwaitingDfEreview;
            }
        }
    }
}
