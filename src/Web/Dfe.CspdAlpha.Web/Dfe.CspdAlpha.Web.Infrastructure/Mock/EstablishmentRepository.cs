using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Dfe.CspdAlpha.Web.Domain.Core;
using Dfe.CspdAlpha.Web.Domain.Entities;
using Dfe.CspdAlpha.Web.Domain.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dfe.CspdAlpha.Web.Infrastructure.Mock
{
    public class EstablishmentRepository : IReadRepository<Establishment>
    {
        private List<Establishment> _establishments;

        class Establishments
        {
            public List<Establishment> establishments { get; set; }
        }

        public EstablishmentRepository()
        {
            _establishments = GetEstablishments().establishments;
        }
        public Establishment GetById(string urn)
        {
            return _establishments.FirstOrDefault(e => e.Urn.Value == urn);
        }

        public List<Establishment> Get()
        {
            throw new NotImplementedException();
        }

        private static string ReadAllText(string resourcePath)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resultJson = string.Empty;
            using (var stream = assembly.GetManifestResourceStream(resourcePath))
            using (var reader = new StreamReader(stream))
            {
                resultJson = reader.ReadToEnd();
            }
            return resultJson;
        }

        private static Establishments GetEstablishments()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resultJson = string.Empty;
            using (var stream = assembly.GetManifestResourceStream("Dfe.CspdAlpha.Web.Infrastructure.Mock.Data.establishments.json"))
            using (var reader = new StreamReader(stream))
            {
                resultJson = reader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<Establishments>(resultJson);
        }
    }
}
