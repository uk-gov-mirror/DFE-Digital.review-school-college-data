using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }

        public List<Pupil> GetByUrn(URN urn)
        {
            throw new NotImplementedException();
        }
    }
}
