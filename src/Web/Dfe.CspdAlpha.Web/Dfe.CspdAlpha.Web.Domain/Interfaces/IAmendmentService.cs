using System;
using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Domain.Entities;

namespace Dfe.CspdAlpha.Web.Domain.Interfaces
{
    public interface IAmendmentService
    {
        IEnumerable<AddPupilAmendment> GetAddPupilAmendments(int laestab);

        AddPupilAmendment GetAddPupilAmendmentDetail(Guid amendmentId);

        bool CreateAddPupilAmendment(AddPupilAmendment amendment);

        bool CancelAddPupilAmendment(Guid amendmentId);
    }
}
