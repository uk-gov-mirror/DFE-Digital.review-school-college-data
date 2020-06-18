using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Domain.Entities;

namespace Dfe.CspdAlpha.Web.Domain.Interfaces
{
    public interface IAmendmentService
    {
        IEnumerable<AddPupilAmendment> GetAddPupilAmendments(int laestab);

        bool CreateAddPupilAmendment(AddPupilAmendment amendment);
    }
}
