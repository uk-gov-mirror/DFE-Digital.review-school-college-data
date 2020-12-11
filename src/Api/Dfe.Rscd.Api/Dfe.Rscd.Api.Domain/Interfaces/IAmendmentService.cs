using System.Collections.Generic;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Domain.Interfaces
{
    public interface IAmendmentService
    {
        IAmendment GetAmendment(CheckingWindow checkingWindow, string id);
        IEnumerable<IDictionary<string, object>> GetAmendments();
        string CreateAmendment(IAmendment amendment);
        void RelateEvidence(string amendmentId, string evidenceFolderName, bool updateEvidenceOption);
        bool CancelAmendment(string amendmentId);
        IEnumerable<IAmendment> GetAmendments(CheckingWindow checkingWindow, string urn);
    }
}