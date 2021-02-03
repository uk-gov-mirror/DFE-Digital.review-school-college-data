using System.Collections.Generic;
using System.Linq;
using Dfe.CspdAlpha.Web.Infrastructure.Crm;
using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Services
{
    public class OutcomeService : IOutcomeService
    {
        private readonly IEnumerable<IRule> _rules;

        public OutcomeService(IEnumerable<IRule> rules)
        {
            _rules = rules;
        }

        public AmendmentOutcome ApplyRules(rscd_Amendment amendmentDto, Amendment amendment)
        {
            var ruleSet = _rules.FirstOrDefault(x => x.AmendmentType == amendment.AmendmentType 
                                                     && x.AmendmentReason == amendment.InclusionReasonId);

            if (ruleSet != null)
            {
                var outcome = ruleSet.Apply(amendment);

                if (amendment.EvidenceStatus == EvidenceStatus.Later)
                {
                    amendmentDto.rscd_Outcome = rscd_Outcome.Awaitingevidence;
                }
                else
                {
                    if (outcome.OutcomeStatus == OutcomeStatus.AutoAccept)
                        amendmentDto.rscd_Outcome = rscd_Outcome.Autoapproved;
                    else if (outcome.OutcomeStatus == OutcomeStatus.AutoReject)
                        amendmentDto.rscd_Outcome = rscd_Outcome.Autorejected;
                    else
                        amendmentDto.rscd_Outcome = rscd_Outcome.AwaitingDfEreview;
                }

                return outcome;
            }

            return new AmendmentOutcome(OutcomeStatus.AwatingDfeReview,
                $"No Accept or Reject rules found for this Reason {amendment.InclusionReasonId}");
        }
    }
}