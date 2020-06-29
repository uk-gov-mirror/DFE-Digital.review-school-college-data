using System;
using System.Collections.Generic;
using System.Text;
using Dfe.CspdAlpha.Web.Domain.Entities;

namespace Dfe.CspdAlpha.Web.Domain.Interfaces
{
    public interface IConfirmationService
    {
        ConfirmationRecord GetConfirmationRecord(string userId, string establishmentId);
        bool UpdateConfirmationRecord(ConfirmationRecord confirmationRecord);
        bool CreateConfirmationRecord(ConfirmationRecord confirmationRecord);
    }
}
