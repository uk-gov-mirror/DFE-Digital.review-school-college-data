using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Domain.Interfaces
{
    public interface IConfirmationService
    {
        ConfirmationRecord GetConfirmationRecord(string userId, string establishmentId);
        bool UpdateConfirmationRecord(ConfirmationRecord confirmationRecord);
        bool CreateConfirmationRecord(ConfirmationRecord confirmationRecord);
    }
}
