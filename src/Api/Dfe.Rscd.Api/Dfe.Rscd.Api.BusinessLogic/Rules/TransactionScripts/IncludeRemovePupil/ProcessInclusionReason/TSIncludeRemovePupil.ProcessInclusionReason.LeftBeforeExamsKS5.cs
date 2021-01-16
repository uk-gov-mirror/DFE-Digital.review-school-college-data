using Web09.Checking.Business.Logic.Entities;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSIncludeRemovePupil
    {
        private static AdjustmentPromptAnalysis ProcessInclusionPromptResponses_LeftBeforeExamsKS5()
        {
            return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment(GetInfoPromptText(5500)));
        }
    }
}
