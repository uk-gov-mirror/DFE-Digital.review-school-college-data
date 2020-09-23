using Dfe.CspdAlpha.Web.Domain.Core;
using Dfe.CspdAlpha.Web.Domain.Entities;
using Dfe.CspdAlpha.Web.Domain.Interfaces;
using Dfe.CspdAlpha.Web.Infrastructure.CosmosDb.DTOs;
using Dfe.CspdAlpha.Web.Infrastructure.CosmosDb.Repositories;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Linq;

namespace Dfe.CspdAlpha.Web.Infrastructure.CosmosDb.Service
{
    public class PupilService : IPupilService
    {
        private Database _cosmosDB;

        public PupilService(CosmosClient cosmosClient, string database)
        {
            _cosmosDB = cosmosClient.GetDatabase(database);
        }

        public Pupil GetById(string checkingWindow, string id)
        {
            var matchedPupil = GetRepository(checkingWindow).Query().Where(p => p.id == id).ToList();
            return matchedPupil.Any() ? matchedPupil.SingleOrDefault().Pupil : null;
        }


        public Pupil GetById(string checkingWindow, PupilId id)
        {
            var matchedPupil = GetRepository(checkingWindow).Query().Where(p => p.UPN == id.Value).ToList();
            return matchedPupil.Any() ? matchedPupil.SingleOrDefault().Pupil : null;
        }

        public List<Pupil> GetByUrn(string checkingWindow, URN urn)
        {
            var dtos = GetRepository(checkingWindow).Query().Where(p => p.URN == urn.Value).ToList();
            return dtos.Select(p => p.Pupil).ToList();
        }
        private IReadRepository<PupilDTO> GetRepository(string checkingWindow)
        {
            var container = _cosmosDB.GetContainer(checkingWindow + "_pupils_2019");
            return new PupilRepository(container);
        }
    }
}
