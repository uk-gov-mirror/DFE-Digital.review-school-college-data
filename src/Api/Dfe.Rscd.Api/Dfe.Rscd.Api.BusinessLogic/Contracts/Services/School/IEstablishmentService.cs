using Dfe.Rscd.Api.BusinessLogic.Contracts.Entities;
using Dfe.Rscd.Api.BusinessLogic.Contracts.Entities.Core;
using Dfe.Rscd.Api.BusinessLogic.Contracts.Entities.Core.Enums;

namespace Dfe.Rscd.Api.BusinessLogic.Contracts.Services
{
    public interface IEstablishmentService
    {
        School GetByURN(CheckingWindow checkingWindow, URN urn);
        School GetByDFESNumber(CheckingWindow checkingWindow, string dfesNumber);
    }
}