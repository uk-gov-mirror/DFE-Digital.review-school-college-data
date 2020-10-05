using System;
using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Domain.Core.Enums;
using Dfe.CspdAlpha.Web.Domain.Entities;

namespace Dfe.CspdAlpha.Web.Domain.Interfaces
{
    public interface IAmendmentService
    {

        bool CreateAddPupilAmendment(AddPupilAmendment amendment, out string id);

        void RelateEvidence(Guid amendmentId, string evidenceFolderName, bool updateEvidenceOption);

        AddPupilAmendment GetAddPupilAmendmentDetail(Guid amendmentId);

        bool CancelAddPupilAmendment(Guid amendmentId);
    }
}
