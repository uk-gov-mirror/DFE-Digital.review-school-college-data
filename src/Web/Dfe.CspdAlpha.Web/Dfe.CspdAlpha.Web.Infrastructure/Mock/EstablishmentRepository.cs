using Dfe.CspdAlpha.Web.Domain.Entities;
using Dfe.CspdAlpha.Web.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Dfe.CspdAlpha.Web.Infrastructure.Mock
{
    public class EstablishmentRepository : BaseRepository, IReadRepository<Establishment>
    {
        private List<Establishment> _establishments;

        public EstablishmentRepository()
        {
            _establishments = GetReourceData<Establishment>("Dfe.CspdAlpha.Web.Infrastructure.Mock.Data.establishments.json");
        }
        public Establishment GetById(string urn)
        {
            return _establishments.FirstOrDefault(e => e.Urn.Value == urn);
        }

        public List<Establishment> Get()
        {
            return _establishments;
        }
        public IQueryable<Establishment> Query()
        {
            return _establishments.AsQueryable();
        }
    }
}
