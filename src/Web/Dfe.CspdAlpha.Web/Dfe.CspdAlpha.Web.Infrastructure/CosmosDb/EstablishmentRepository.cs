using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Dfe.CspdAlpha.Web.Domain.Core;
using Dfe.CspdAlpha.Web.Domain.Entities;
using Dfe.CspdAlpha.Web.Domain.Interfaces;
using Dfe.CspdAlpha.Web.Infrastructure.Mock;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json.Linq;

namespace Dfe.CspdAlpha.Web.Infrastructure.CosmosDb
{
    public class EstablishmentRepository : IReadRepository<Establishment>
    {
        private Container _container { get; }

        public EstablishmentRepository(CosmosClient cosmosClient, string database, string collection)
        {
            _container = cosmosClient.GetContainer(database, collection);
        }

        public Establishment GetById(string id)
        {
            var result = _container.ReadItemAsync<dynamic>(id, PartitionKey.None).Result;
            if (result.StatusCode == HttpStatusCode.OK && result.Resource is JObject)
            {
                return ConvertToEstablishment(result.Resource as JObject, id);
            }

            return null;
        }

        private Establishment ConvertToEstablishment(JObject establishment, string id)
        {
            return new Establishment
            {
                Urn = new URN(id),
                LaEstab = establishment.Value<int>("DFESNumber").ToString(),
                Name = establishment.Value<string>("SchoolName"),
                CohortMeasures = new List<PerformanceMeasure>(),
                PerformanceMeasures = establishment.Value<JArray>("performance").Select(p => new PerformanceMeasure
                    { Name = p.Value<string>("Code"), Value = p.Value<string>("CodeValue") }).ToList()
            };
        }

        public List<Establishment> Get()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Establishment> Query()
        {
            throw new NotImplementedException();
        }
    }
}
