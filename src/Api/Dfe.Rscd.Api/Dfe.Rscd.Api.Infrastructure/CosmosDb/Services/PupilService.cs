using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.Domain.Core;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Interfaces;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.DTOs;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Repositories;
using Microsoft.Extensions.Configuration;

namespace Dfe.Rscd.Api.Infrastructure.CosmosDb.Services
{
    public class PupilService : IPupilService
    {
        // TODO: Decide a max pagesize for now, can't return all pupils
        private const int PageSize = 200;
        private readonly string _allocationYear;
        private readonly IRepository _repository;

        public PupilService(IRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _allocationYear = configuration["AllocationYear"];
        }

        public Pupil GetById(CheckingWindow checkingWindow, string id)
        {
            var pupilDTO = _repository.GetById<PupilDTO>(GetCollection(checkingWindow), id);
            return pupilDTO?.GetPupil(_allocationYear);
        }

        public List<PupilRecord> QueryPupils(CheckingWindow checkingWindow, PupilsSearchRequest query)
        {
            var repoQuery = _repository.Get<PupilDTO>(GetCollection(checkingWindow));
            if (!string.IsNullOrWhiteSpace(query.URN)) repoQuery = repoQuery.Where(p => p.URN == query.URN);
            if (!string.IsNullOrWhiteSpace(query.ID))
                repoQuery = repoQuery.Where(p => p.UPN.StartsWith(query.ID) || p.ULN.StartsWith(query.ID));
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                var nameParts = query.Name.Split(' ');
                foreach (var namePart in nameParts)
                    repoQuery = repoQuery.Where(p => p.Forename.StartsWith(namePart) || p.Surname.StartsWith(namePart));
            }


            var dtos = repoQuery
                .Select(p => new PupilRecord
                    {Id = p.id, ForeName = p.Forename, Surname = p.Surname, ULN = p.ULN, UPN = p.UPN})
                .Take(PageSize);

            return dtos.ToList();
        }

        private string GetCollection(CheckingWindow checkingWindow)
        {
            return $"{checkingWindow.ToString().ToLower()}_pupils_{_allocationYear}";
        }
    }
}