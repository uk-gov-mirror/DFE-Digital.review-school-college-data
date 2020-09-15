using System;
using System.Collections.Generic;
using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Domain.Interfaces
{
    public interface IAmendmentService
    {
        IEnumerable<AddPupilAmendment> GetAddPupilAmendments(int laestab);
        IEnumerable<AddPupilAmendment> GetAddPupilAmendments(string urn);

        bool CreateAddPupilAmendment(AddPupilAmendment amendment, out string id);
        void RelateEvidence(Guid amendmentId, List<Evidence> evidenceList, bool updateEvidenceOption);
        AddPupilAmendment GetAddPupilAmendmentDetail(Guid amendmentId);


        bool CancelAddPupilAmendment(Guid amendmentId);
    }
}
