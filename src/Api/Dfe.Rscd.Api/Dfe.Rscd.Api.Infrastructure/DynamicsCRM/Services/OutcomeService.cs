using System.Collections.Generic;
using System.Linq;
using Dfe.CspdAlpha.Web.Infrastructure.Crm;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Interfaces;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Interfaces;

namespace Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Services
{
    public class OutcomeService : IOutcomeService
    {
        private readonly IEnumerable<IRuleSet> _rules;

        public OutcomeService(IEnumerable<IRuleSet> rules)
        {
            _rules = rules;
        }

        public void SetOutcome(rscd_Amendment amendmentDto, IAmendment amendment)
        {
            if (amendment.EvidenceStatus == EvidenceStatus.Later)
            {
                amendmentDto.rscd_Outcome = rscd_Outcome.Awaitingevidence;
            }
            else
            {
                var ruleSet = _rules.FirstOrDefault(x => x.AmendmentType == amendment.AmendmentType);

                var outcome = ruleSet.Apply(amendment);

                if (outcome == OutcomeStatus.AutoAccept)
                    amendmentDto.rscd_Outcome = rscd_Outcome.Autoapproved;
                else if (outcome == OutcomeStatus.AutoReject)
                    amendmentDto.rscd_Outcome = rscd_Outcome.Autorejected;
                else
                    amendmentDto.rscd_Outcome = rscd_Outcome.AwaitingDfEreview;
            }
        }
    }
}