using System;
using System.Collections.Generic;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Domain.Interfaces
{
    public interface IAmendmentService
    {
        IEnumerable<IDictionary<string, object>> GetAmendments();

        IEnumerable<AddPupilAmendment> GetAddPupilAmendments(CheckingWindow checkingWindow, string urn);

        string CreateAmendment(Amendment amendment);







        IEnumerable<AddPupilAmendment> GetAddPupilAmendments(int laestab);

        bool CreateAddPupilAmendment(CheckingWindow checkingWindow, AddPupilAmendment amendment, out string id);
        void RelateEvidence(Guid amendmentId, List<Evidence> evidenceList, bool updateEvidenceOption);
        AddPupilAmendment GetAddPupilAmendmentDetail(Guid amendmentId);


        bool CancelAddPupilAmendment(Guid amendmentId);
    }
}
