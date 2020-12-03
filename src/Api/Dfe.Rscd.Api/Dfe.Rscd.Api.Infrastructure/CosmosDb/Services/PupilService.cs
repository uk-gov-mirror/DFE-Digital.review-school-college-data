using Dfe.Rscd.Api.Domain.Core;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Interfaces;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Config;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Repositories;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace Dfe.Rscd.Api.Infrastructure.CosmosDb.Services
{
    public class PupilService : IPupilService
    {
        private Database _cosmosDb;
        private readonly string ALLOCATION_YEAR;

        // TODO: Decide a max pagesize for now, can't return all pupils
        private const int PageSize = 200;

        public PupilService(IOptions<CosmosDbOptions> options, IConfiguration configuration )
        {
            _cosmosDb = new CosmosClient(options.Value.Account, options.Value.Key).GetDatabase(options.Value.Database);
            ALLOCATION_YEAR = configuration["AllocationYear"];
        }

        public Pupil GetById(CheckingWindow checkingWindow, string id)
        {
            var matchedPupil = GetRepository(checkingWindow).GetById(id);
            return matchedPupil.GetPupil(ALLOCATION_YEAR);
        }

        public List<PupilRecord> QueryPupils(CheckingWindow checkingWindow, PupilsSearchRequest query)
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

            var dtos = repoQuery
                .Select(p => new PupilRecord { Id = p.id, ForeName = p.Forename, Surname = p.Surname, ULN = p.ULN, UPN = p.UPN })
                .Take(PageSize);
            
            return dtos.ToList();
        }

        private PupilRepository GetRepository(CheckingWindow checkingWindow)
        {
            var container = _cosmosDb.GetContainer( $"{checkingWindow.ToString().ToLower()}_pupils_{ALLOCATION_YEAR}");
            return new PupilRepository(container);
        }
    }
}
