using System.Collections.Generic;
using System.Linq;
using Dfe.CspdAlpha.Web.Infrastructure.Crm;
using Dfe.Rscd.Api.BusinessLogic.Entities;

namespace Dfe.Rscd.Api.Services
{
    public class OutcomeService : IOutcomeService
    {
        private readonly IEnumerable<IRuleSet> _rules;

        public OutcomeService(IEnumerable<IRuleSet> rules)
        {
            _rules = rules;
        }

        public void SetOutcome(rscd_Amendment amendmentDto, Amendment amendment)
        {
            if (amendment.EvidenceStatus == EvidenceStatus.Later)
            {
                amendmentDto.rscd_Outcome = rscd_Outcome.Awaitingevidence;
            }
            else
            {
                var ruleSet = _rules.FirstOrDefault(x => x.AmendmentType == amendment.AmendmentType);

                var outcome = ruleSet.Apply(amendment);

                if (outcome.CompleteSimpleOutcome.OutcomeStatus == OutcomeStatus.AutoAccept)
                    amendmentDto.rscd_Outcome = rscd_Outcome.Autoapproved;
                else if (outcome.CompleteSimpleOutcome.OutcomeStatus == OutcomeStatus.AutoReject)
                    amendmentDto.rscd_Outcome = rscd_Outcome.Autorejected;
                else
                    amendmentDto.rscd_Outcome = rscd_Outcome.AwaitingDfEreview;
            }
        }
    }
}