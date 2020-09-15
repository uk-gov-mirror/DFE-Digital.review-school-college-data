using Dfe.Rscd.Api.Domain.Core;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Interfaces;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.DTOs;
using System.Linq;

namespace Dfe.Rscd.Api.Infrastructure.CosmosDb.Services
{
    public class EstablishmentService : IEstablishmentService
    {
        private IReadRepository<EstablishmentsDTO> _establishmentRepository;

        public EstablishmentService(IReadRepository<EstablishmentsDTO> establishmentRepository)
        {
            _establishmentRepository = establishmentRepository;
        }
        public Establishment GetByURN(URN urn)
        {
            return _establishmentRepository.GetById(urn.Value).Establishment;
        }

        public Establishment GetByLAId(string laId)
        {
            var results = _establishmentRepository.Query().Where(e => e.DFESNumber == laId).ToList();
            return results.Count > 0 ? results.First().Establishment : null;
        }
    }
}
