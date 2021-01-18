using Dfe.Rscd.Api.BusinessLogic.Entities;

namespace Dfe.Rscd.Api.Services
{
    public interface IPromptService
    {
        AdjustmentOutcome GetAdjustmentPrompts(CheckingWindow checkingWindow, int dfesNumber, string studendId,
            int inclusionReasonId);

        AmendmentType AmendmentType { get; }
    }
}