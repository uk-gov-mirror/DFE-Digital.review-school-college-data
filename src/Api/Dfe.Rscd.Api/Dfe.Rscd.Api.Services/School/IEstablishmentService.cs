using Dfe.Rscd.Api.BusinessLogic.Entities;

namespace Dfe.Rscd.Api.Services
{
    public interface IEstablishmentService
    {
        School GetByURN(CheckingWindow checkingWindow, URN urn);
        School GetByDFESNumber(CheckingWindow checkingWindow, string dfesNumber);
    }
}