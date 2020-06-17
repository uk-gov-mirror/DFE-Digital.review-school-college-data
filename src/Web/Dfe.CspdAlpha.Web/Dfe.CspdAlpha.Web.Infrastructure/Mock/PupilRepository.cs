using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dfe.CspdAlpha.Web.Domain.Entities;
using Dfe.CspdAlpha.Web.Domain.Interfaces;

namespace Dfe.CspdAlpha.Web.Infrastructure.Mock
{
    public class PupilRepository: BaseRepository, IReadRepository<Pupil>
    {
        private List<Pupil> _pupils;

        public PupilRepository()
        {
            _pupils = GetReourceData<Pupil>("Dfe.CspdAlpha.Web.Infrastructure.Mock.Data.pupils.json");
        }
        public Pupil GetById(string id)
        {
            return _pupils.FirstOrDefault(e => e.Id.Value == id);
        }

        public List<Pupil> Get()
        {
            return _pupils;
        }

        public IQueryable<Pupil> Query()
        {
            return _pupils.AsQueryable();
        }
    }
}
