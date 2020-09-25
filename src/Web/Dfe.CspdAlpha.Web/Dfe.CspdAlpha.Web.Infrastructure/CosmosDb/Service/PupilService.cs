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

        public List<Pupil> QueryPupils(string checkingWindow, PupilQuery query)
        {
            var repoQuery = GetRepository(checkingWindow).Query();
            if (!string.IsNullOrWhiteSpace(query.URN))
            {
                repoQuery = repoQuery.Where(p => p.URN == query.URN);
            }
            if (!string.IsNullOrWhiteSpace(query.ID))
            {
                repoQuery = repoQuery.Where(p => p.UPN.StartsWith(query.ID) || p.ULN.StartsWith(query.ID));
            }
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                var nameParts = query.Name.Split(' ');
                foreach (var namePart in nameParts)
                {
                    repoQuery = repoQuery.Where(p => p.Forename.StartsWith(namePart) || p.Surname.StartsWith(namePart));
                }
            }

            var dtos = repoQuery.ToList();
            return dtos.Select(p => p.Pupil).ToList();
        }

        private IReadRepository<PupilDTO> GetRepository(string checkingWindow)
        {
            var container = _cosmosDB.GetContainer(checkingWindow + "_pupils_2019");
            return new PupilRepository(container);
        }
    }
}
