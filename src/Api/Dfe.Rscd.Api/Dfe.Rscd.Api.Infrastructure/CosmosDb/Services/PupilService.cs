using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dfe.Rscd.Api.Domain.Core;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Interfaces;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.DTOs;

namespace Dfe.Rscd.Api.Infrastructure.CosmosDb.Services
{
    public class PupilService : IPupilService
    {
        private IReadRepository<PupilDTO> _pupilRepository;

        public PupilService(IReadRepository<PupilDTO> pupilRepository)
        {
            _pupilRepository = pupilRepository;
        }

        public Pupil GetById(PupilId id)
        {
            var matchedPupil = _pupilRepository.Query().Where(p => p.UPN == id.Value).ToList();
            return matchedPupil.Any() ? matchedPupil.SingleOrDefault().Pupil : null;
        }

        public List<Pupil> GetByUrn(URN urn)
        {
            var dtos = _pupilRepository.Query().Where(p => p.URN == urn.Value).ToList();
            return dtos.Select(p => p.Pupil).ToList();
        }

        public List<Pupil> FindMatchedPupils(Pupil pupil)
        {
            var dob = int.Parse(pupil.DateOfBirth.ToString("yyyyMMdd"));
            var matchedPupils = _pupilRepository.Query().Where(p =>
               p.Forename == pupil.ForeName && p.Surname == pupil.LastName && p.DOB == dob).ToList();
            return matchedPupils.Select(p => p.Pupil).ToList();
        }
    }
}
