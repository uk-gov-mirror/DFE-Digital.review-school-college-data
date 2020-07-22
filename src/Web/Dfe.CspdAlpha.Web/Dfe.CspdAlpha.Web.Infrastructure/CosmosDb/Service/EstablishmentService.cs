﻿using Dfe.CspdAlpha.Web.Domain.Core;
using Dfe.CspdAlpha.Web.Domain.Entities;
using Dfe.CspdAlpha.Web.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dfe.CspdAlpha.Web.Infrastructure.CosmosDb.Service
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
            return _establishmentRepository.GetById(urn.Value);
        }

        public List<Establishment> GetByLAId(string laId)
        {
            throw new NotImplementedException();
        }
    }
}