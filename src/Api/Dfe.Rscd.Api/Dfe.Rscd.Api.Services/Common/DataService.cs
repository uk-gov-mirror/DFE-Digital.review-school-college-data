using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.BusinessLogic.Entities;
using Dfe.Rscd.Api.Infrastructure.SqlServer.Repositories;
using DTO = Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs;

namespace Dfe.Rscd.Api.Services
{
    public class DataService : IDataService
    {
        private readonly IDataRepository _repository;

        public DataService(IDataRepository repository)
        {
            _repository = repository;
        }

        public IList<AmendCode> GetAmendCodes()
        {
            return _repository
                .Get<DTO.AmendCode>()
                .Select(x => new AmendCode
                {
                    Code = x.AmendCode1,
                    Description = x.AmendCodeDescription
                })
                .ToList();
        }

        public IList<AwardingBody> GetAwardingBodies()
        {
            return _repository
                .Get<DTO.AwardingBody>()
                .Select(x => new AwardingBody
                {
                    AwardingBodyID = x.AwardingBodyId,
                    AwardingBodyName = x.AwardingBodyName
                })
                .ToList();
        }

        public IList<Ethnicity> GetEthnicities()
        {
            return _repository
                .Get<DTO.Ethnicity>()
                .Select(x => new Ethnicity
                {
                    Code = x.EthnicityCode, 
                    Description = x.EthnicityDescription
                })
                .ToList();
        }

        public IList<InclusionAdjustmentReason> GetInclusionAdjustmentReasons()
        {
            return _repository
                .Get<DTO.InclusionAdjustmentReason>()
                .Select(x=> new InclusionAdjustmentReason
                {
                    IncAdjReasonId = x.IncAdjReasonId,
                    CanCancel = x.CanCancel,
                    InJuneChecking = x.InJuneChecking, 
                    IncAdjReasonDescription = x.IncAdjReasonDescription, 
                    IsInclusion = x.IsInclusion, 
                    IsNewStudentReason = x.IsNewStudentReason, 
                    ListOrder = x.ListOrder
                })
                .ToList();
        }

        public IList<FirstLanguage> GetLanguages()
        {
            return _repository.Get<DTO.Language>()
                .Select(x => new FirstLanguage
                {
                    Code = x.LanguageCode, 
                    Description = x.LanguageDescription
                })
                .ToList();
        }

        public IList<PINCLs> GetPINCLs()
        {
            return _repository.Get<DTO.Pincl>()
                .Select(x => new PINCLs
                {
                    P_INCL = x.PIncl1, 
                    P_INCLDescription = x.PIncldescription, 
                    DisplayFlag = x.DisplayFlag
                })
                .ToList();
        }

        public IList<SENStatus> GetSENStatus()
        {
            return _repository.Get<DTO.Senstatus>()
                .Select(x => new SENStatus
                {
                    Code = x.SenstatusCode, 
                    Description = x.SenstatusDescription
                })
                .ToList();
        }

        public IList<YearGroup> GetYearGroups()
        {
            return _repository.Get<DTO.YearGroup>()
                .Select(x=> new YearGroup
                {
                    YearGroupCode = x.YearGroupCode, 
                    YearGroupDescription = x.YearGroupDescription
                })
                .ToList();
        }
    }
}