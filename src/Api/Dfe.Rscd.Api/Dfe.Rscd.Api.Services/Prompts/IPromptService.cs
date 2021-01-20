using Dfe.Rscd.Api.BusinessLogic.Entities;

namespace Dfe.Rscd.Api.Services
{
    public interface IPromptService
    {
        AmendmentOutcome GetAdjustmentPrompts(CheckingWindow checkingWindow, string pinclCode, int inclusionReasonId);

        AmendmentType AmendmentType { get; }
    }
}