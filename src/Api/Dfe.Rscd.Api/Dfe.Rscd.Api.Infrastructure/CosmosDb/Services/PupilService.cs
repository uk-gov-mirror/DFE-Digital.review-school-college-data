using Dfe.Rscd.Api.Domain.Core;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Interfaces;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Repositories;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Linq;

namespace Dfe.Rscd.Api.Infrastructure.CosmosDb.Services
{
    public class PupilService : IPupilService
    {
        private Database _cosmosDb;

        public PupilService(Database cosmosDB)
        {
            _cosmosDb = cosmosDB;
        }

        public Pupil GetById(CheckingWindow checkingWindow, string id)
        {
            var matchedPupil = GetRepository(checkingWindow).GetById(id);
            return matchedPupil.Pupil;
        }

        public List<Pupil> QueryPupils(CheckingWindow checkingWindow, PupilsSearchRequest query)
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

        private PupilRepository GetRepository(CheckingWindow checkingWindow)
        {
            var container = _cosmosDb.GetContainer(checkingWindow.ToString().ToLower() + "_pupils_2019");
            return new PupilRepository(container);
        }
    }
}
