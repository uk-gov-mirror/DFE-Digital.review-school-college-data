using System.Collections.Generic;

namespace Dfe.Rscd.Api.BusinessLogic.Entities
{
    public interface IRuleSet
    {
        AmendmentType AmendmentType { get; }
        AdjustmentOutcome Apply(Amendment amendment);

        RuleSetContext Context { get; set; }
    }

    public class RuleSetContext
    {
        public int InclusionReasonId { get; set; }
        public List<PromptAnswer> PromptAnswers { get; set; }
        public string DfesNumber { get; set; }
        public Pupil Pupil { get;set; }
    }
}