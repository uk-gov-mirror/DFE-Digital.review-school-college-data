using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Entities.Questions;
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
                    Id = x.AwardingBodyId,
                    Name = x.AwardingBodyName
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

        public IList<AmendmentReason> GetAmendmentReasons(CheckingWindow checkingWindow, AmendmentType amendmentType)
        {
            return new List<AmendmentReason>
            {
                new AmendmentReason{Description ="Admitted following permanent exclusion from a maintained school", ReasonId = 10, AmendmentType = AmendmentType.RemovePupil},
                new AmendmentReason{Description ="Admitted from abroad with English not first language", ReasonId = 8, AmendmentType = AmendmentType.RemovePupil},
                new AmendmentReason{Description ="Deceased", ReasonId = 12, AmendmentType = AmendmentType.RemovePupil},
                new AmendmentReason{Description ="Permanently left England", ReasonId = 11, AmendmentType = AmendmentType.RemovePupil},
                new AmendmentReason{Description ="Other", ReasonId = 19, AmendmentType = AmendmentType.RemovePupil},
                new AmendmentReason{Description ="Other - EAL exceptional circumstances", ReasonId = 1901, ParentReasonId = 19, AmendmentType = AmendmentType.RemovePupil},
                new AmendmentReason{Description ="Other - Elective home education", ReasonId = 1902, ParentReasonId = 19, AmendmentType = AmendmentType.RemovePupil},
                new AmendmentReason{Description ="Other - In prison/remand centre/secure unit", ReasonId = 1903, ParentReasonId = 19, AmendmentType = AmendmentType.RemovePupil},
                new AmendmentReason{Description ="Other - Permanently excluded from this school", ReasonId = 1904, ParentReasonId = 19, AmendmentType = AmendmentType.RemovePupil},
                new AmendmentReason{Description ="Other - Police involvement/bail restrictions", ReasonId = 1905, ParentReasonId = 19, AmendmentType = AmendmentType.RemovePupil},
                new AmendmentReason{Description ="Other - Pupil missing in education", ReasonId = 1906, ParentReasonId = 19, AmendmentType = AmendmentType.RemovePupil},
                new AmendmentReason{Description ="Other - Safeguarding/FAP", ReasonId = 1907, ParentReasonId = 19, AmendmentType = AmendmentType.RemovePupil},
                new AmendmentReason{Description ="Other - Terminal/Long illness", ReasonId = 1908, ParentReasonId = 19, AmendmentType = AmendmentType.RemovePupil}
            }.Where(x => x.AmendmentType == amendmentType).ToList();
        }

        public IList<AnswerPotential> GetAnswerPotentials(string questionId)
        {
            return _repository.Get<DTO.PotentialAnswer>()
                .Where(x=> x.QuestionId == questionId)
                .Select(x => new AnswerPotential
                {
                    Value = x.Id.ToString(),
                    Description = x.AnswerValue,
                    Reject = x.IsRejected
                })
                .ToList();
        }

        public IList<PInclude> GetPINCLs()
        {
            return _repository.Get<DTO.Pincl>()
                .Select(x => new PInclude
                {
                    Code = x.PIncl1,
                    Description = x.PIncldescription,
                    DisplayFlag = x.DisplayFlag
                })
                .ToList();
        }

        public IList<SpecialEducationNeed> GetSENStatus()
        {
            return _repository.Get<DTO.Senstatus>()
                .Select(x => new SpecialEducationNeed
                {
                    Code = x.SenstatusCode,
                    Description = x.SenstatusDescription
                })
                .ToList();
        }

        public IList<YearGroup> GetYearGroups()
        {
            return _repository.Get<DTO.YearGroup>()
                .Select(x => new YearGroup
                {
                    YearGroupCode = x.YearGroupCode,
                    YearGroupDescription = x.YearGroupDescription
                })
                .ToList();
        }
    }
}