using Dfe.Rscd.Api.Domain.Core;
using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Domain.Interfaces
{
    public interface IEstablishmentService
    {
        Establishment GetByURN(URN urn);
        Establishment GetByLAId(string laId);
    }
}
