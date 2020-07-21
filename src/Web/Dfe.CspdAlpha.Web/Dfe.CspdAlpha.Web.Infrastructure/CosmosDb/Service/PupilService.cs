using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dfe.CspdAlpha.Web.Domain.Core;
using Dfe.CspdAlpha.Web.Domain.Entities;
using Dfe.CspdAlpha.Web.Domain.Interfaces;

namespace Dfe.CspdAlpha.Web.Infrastructure.CosmosDb.Service
{
    public class PupilService : IPupilService
    {
        private IReadRepository<PupilDTO> _pupilRepository;

        public PupilService(IReadRepository<PupilDTO> pupilRepository)
        {
            _pupilRepository = pupilRepository;
        }

        public Pupil GetById(PupilId id)
        {
            return _pupilRepository.GetById(id.Value).Pupil;
        }

        public List<Pupil> GetByUrn(URN urn)
        {
            var dtos = _pupilRepository.Query().Where(p => p.URN == urn.Value).ToList();
            return dtos.Select(p => p.Pupil).ToList();
        }
    }
}
