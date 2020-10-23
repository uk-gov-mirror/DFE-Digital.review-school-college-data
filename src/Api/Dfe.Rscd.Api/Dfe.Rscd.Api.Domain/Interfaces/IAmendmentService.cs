using System.Collections.Generic;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Domain.Interfaces
{
    public interface IAmendmentService
    {
        Amendment GetAmendment(CheckingWindow checkingWindow, string id);
        IEnumerable<IDictionary<string, object>> GetAmendments();
        string CreateAmendment(Amendment amendment);
        void RelateEvidence(string amendmentId, string evidenceFolderName, bool updateEvidenceOption);
        bool CancelAmendment(string amendmentId);
        IEnumerable<Amendment> GetAmendments(CheckingWindow checkingWindow, string urn);
    }
}
