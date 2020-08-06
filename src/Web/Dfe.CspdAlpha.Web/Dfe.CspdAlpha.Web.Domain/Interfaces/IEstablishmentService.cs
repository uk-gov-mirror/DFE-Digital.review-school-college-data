using System;
using System.Collections.Generic;
using System.Text;
using Dfe.CspdAlpha.Web.Domain.Core;
using Dfe.CspdAlpha.Web.Domain.Entities;

namespace Dfe.CspdAlpha.Web.Domain.Interfaces
{
    public interface IEstablishmentService
    {
        Establishment GetByURN(URN urn);
        Establishment GetByLAId(string laId);
    }
}
