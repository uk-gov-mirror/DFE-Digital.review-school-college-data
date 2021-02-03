namespace Dfe.Rscd.Api.Domain.Entities
{
    public interface IQuestionProvider
    {
        AmendmentOutcome GetQuestions(CheckingWindow checkingWindow, string pinclCode, int inclusionReasonId);

        AmendmentType AmendmentType { get; }

        int AmendmentReason { get; }
    }
}