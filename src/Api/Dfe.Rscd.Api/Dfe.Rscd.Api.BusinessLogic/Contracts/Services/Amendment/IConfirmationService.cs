using Dfe.Rscd.Api.BusinessLogic.Contracts.Entities.Amendments;

namespace Dfe.Rscd.Api.BusinessLogic.Contracts.Services
{
    public interface IConfirmationService
    {
        ConfirmationRecord GetConfirmationRecord(string userId, string establishmentId);
        bool UpdateConfirmationRecord(ConfirmationRecord confirmationRecord);
        bool CreateConfirmationRecord(ConfirmationRecord confirmationRecord);
    }
}