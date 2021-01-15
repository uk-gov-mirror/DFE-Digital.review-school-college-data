using System.Collections.Generic;
using Dfe.Rscd.Api.BusinessLogic.Contracts.Entities.Core.Enums;

namespace Dfe.Rscd.Api.BusinessLogic.Contracts.Services
{
    public interface IAmendmentService
    {
        Entities.Amendments.Amendment GetAmendment(CheckingWindow checkingWindow, string id);
        IEnumerable<IDictionary<string, object>> GetAmendments();
        string AddAmendment(Entities.Amendments.Amendment amendment);
        void RelateEvidence(string amendmentId, string evidenceFolderName, bool updateEvidenceOption);
        bool CancelAmendment(string amendmentId);
        IEnumerable<Entities.Amendments.Amendment> GetAmendments(CheckingWindow checkingWindow, string urn);
    }
}