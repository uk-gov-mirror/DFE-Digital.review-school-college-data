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
            else
            {
                var outcome = amendment.AmendmentDetail.GetOutcomeStatus(amendment);

                if (outcome == OutcomeStatus.AutoAccept)
                {
                    amendmentDto.rscd_Outcome = rscd_Outcome.Autoapproved;
                    amendmentDto.rscd_Amendmentstatus = new_amendmentstatus.Accepted;
                }
                else if (outcome == OutcomeStatus.AutoReject)
                {
                    amendmentDto.rscd_Outcome = rscd_Outcome.Autorejected;
                    amendmentDto.rscd_Amendmentstatus = new_amendmentstatus.Rejected;
                }
                else
                {
                    amendmentDto.rscd_Outcome = rscd_Outcome.AwaitingDfEreview;
                }
            }
        }
    }
}
