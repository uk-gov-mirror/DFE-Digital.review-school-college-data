using Dfe.CspdAlpha.Web.Domain.Core;
using Dfe.CspdAlpha.Web.Domain.Entities;
using Dfe.CspdAlpha.Web.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dfe.CspdAlpha.Web.Infrastructure.CosmosDb.DTOs;

namespace Dfe.CspdAlpha.Web.Infrastructure.CosmosDb.Service
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
