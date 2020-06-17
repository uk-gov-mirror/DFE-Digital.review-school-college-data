using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dfe.CspdAlpha.Web.Domain.Core;
using Dfe.CspdAlpha.Web.Domain.Entities;
using Dfe.CspdAlpha.Web.Domain.Interfaces;

namespace Dfe.CspdAlpha.Web.Domain.Services
{
    public class PupilService : IPupilService
    {
        private IReadRepository<Pupil> _pupilRepository;

        public PupilService(IReadRepository<Pupil> pupilRepository)
        {
            _pupilRepository = pupilRepository;
        }
        public Pupil GetById(PupilId id)
        {
            return _pupilRepository.GetById(id.Value);
        }

        public List<Pupil> GetByUrn(URN urn)
        {
            return _pupilRepository.Query().Where(p => p.Urn.Value == urn.Value).ToList();
        }
    }
}
