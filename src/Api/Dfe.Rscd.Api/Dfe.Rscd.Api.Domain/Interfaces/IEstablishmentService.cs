using Dfe.Rscd.Api.Domain.Core;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Domain.Interfaces
{
    public interface IEstablishmentService
    {
        Establishment GetByURN(CheckingWindow checkingWindow, URN urn);
        Establishment GetByLAId(CheckingWindow checkingWindow, string laId);
    }
}
