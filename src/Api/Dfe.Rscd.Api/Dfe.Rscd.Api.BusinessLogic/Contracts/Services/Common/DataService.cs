using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.BusinessLogic.Contracts.Entities;
using Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs;
using Dfe.Rscd.Api.Infrastructure.SqlServer.Repositories;
using AwardingBody = Dfe.Rscd.Api.BusinessLogic.Contracts.Entities.AwardingBody;
using Ethnicity = Dfe.Rscd.Api.BusinessLogic.Contracts.Entities.Ethnicity;
using DTO = Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs;

namespace Dfe.Rscd.Api.BusinessLogic.Contracts.Services
{
    public class DataService : IDataService
    {
        private readonly IDataRepository _repository;

        public DataService(IDataRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<T> Query<T>() where T : class
        {
            return _repository.Get<T>();
        }

        public IList<AmendCode> GetAmendCodes()
        {
            return _repository.Get<DTO.AmendCode>().ToList();
        }

        public IList<Entities.AwardingBody> GetAwardingBodies()
        {
            return _repository.Get<DTO.AwardingBody>()
                .Select(x=> new AwardingBody {AwardingBodyID = x.AwardingBodyId, AwardingBodyName = x.AwardingBodyName})
                .ToList();
        }

        public IList<Ethnicity> GetEthnicities()
        {
            return _repository.Get<DTO.Ethnicity>()
                .Select(x=> new Ethnicity { Code = x.EthnicityCode, Description = x.EthnicityDescription})
                .ToList();
        }

        public IList<InclusionAdjustmentReason> GetInclusionAdjustmentReasons()
        {
            return _repository.Get<DTO.InclusionAdjustmentReason>()
                .ToList();
        }

        public IList<FirstLanguage> GetLanguages()
        {
            return _repository.Get<DTO.Language>()
                .Select(x=> new FirstLanguage {Code = x.LanguageCode, Description = x.LanguageDescription})
                .ToList();
        }

        public IList<PINCLs> GetPINCLs()
        {
            return _repository.Get<DTO.Pincl>()
                .Select(x => new PINCLs{P_INCL = x.PIncl1, P_INCLDescription = x.PIncldescription, DisplayFlag = x.DisplayFlag})
                .ToList();
        }

        public IList<SENStatus> GetSENStatus()
        {
            return _repository.Get<DTO.Senstatus>()
                .Select(x=> new SENStatus{Code = x.SenstatusCode, Description = x.SenstatusDescription})
                .ToList();
        }

        public IList<YearGroup> GetYearGroups()
        {
            return _repository.Get<DTO.YearGroup>()
                .ToList();
        }
    }
}