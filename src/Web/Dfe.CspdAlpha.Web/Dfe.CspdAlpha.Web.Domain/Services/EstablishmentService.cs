using System;
using System.Collections.Generic;
using System.Text;
using Dfe.CspdAlpha.Web.Domain.Core;
using Dfe.CspdAlpha.Web.Domain.Entities;
using Dfe.CspdAlpha.Web.Domain.Interfaces;

namespace Dfe.CspdAlpha.Web.Domain.Services
{
    public class EstablishmentService : IEstablishmentService
    {
        private IReadRepository<Establishment> _establishmentRepository;

        public EstablishmentService(IReadRepository<Establishment> establishmentRepository)
        {
            _establishmentRepository = establishmentRepository;
        }
        public Establishment GetByURN(URN urn)
        {
            throw new NotImplementedException();
        }

        public List<Establishment> GetByLAId(string laId)
        {
            throw new NotImplementedException();
        }
    }
}
