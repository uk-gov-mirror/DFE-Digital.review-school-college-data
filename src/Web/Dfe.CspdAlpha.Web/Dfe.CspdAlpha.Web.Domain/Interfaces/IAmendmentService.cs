using System;
using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Domain.Entities;

namespace Dfe.CspdAlpha.Web.Domain.Interfaces
{
    public interface IAmendmentService
    {
        IEnumerable<AddPupilAmendment> GetAddPupilAmendments(int laestab);
        IEnumerable<AddPupilAmendment> GetAddPupilAmendments(string urn);

        bool CreateAddPupilAmendment(AddPupilAmendment amendment, out string id);
        AddPupilAmendment GetAddPupilAmendmentDetail(Guid amendmentId);


        bool CancelAddPupilAmendment(Guid amendmentId);
    }
}
