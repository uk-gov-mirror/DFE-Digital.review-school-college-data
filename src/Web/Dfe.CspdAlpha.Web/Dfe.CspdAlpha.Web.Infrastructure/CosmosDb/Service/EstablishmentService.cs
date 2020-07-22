using Dfe.CspdAlpha.Web.Domain.Core;
using Dfe.CspdAlpha.Web.Domain.Entities;
using Dfe.CspdAlpha.Web.Domain.Interfaces;
using System;
using System.Collections.Generic;
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

        public List<Establishment> GetByLAId(string laId)
        {
            throw new NotImplementedException();
        }
    }
}
