using System;
using System.Collections.Generic;
using System.Text;
using Dfe.CspdAlpha.Web.Domain.Core;
using Dfe.CspdAlpha.Web.Domain.Entities;
using Dfe.CspdAlpha.Web.Domain.Interfaces;

namespace Dfe.CspdAlpha.Web.Domain.Services
{
    public class AmendmentService : IAmendmentService
    {
        private IWriteRepository<AmendmentRecord> _amemdmentRepository;

        public AmendmentService(IWriteRepository<AmendmentRecord> amemdmentRepository)
        {
            _amemdmentRepository = amemdmentRepository;
        }
        public IEnumerable<AmendmentRecord> Get(PupilId id)
        {
            throw new NotImplementedException();
        }

        public string Create(IEvent newEvent, Audit audit)
        {
            throw new NotImplementedException();
        }

        public void Update(IEvent newEvent, Audit audit)
        {
            throw new NotImplementedException();
        }
    }
}
