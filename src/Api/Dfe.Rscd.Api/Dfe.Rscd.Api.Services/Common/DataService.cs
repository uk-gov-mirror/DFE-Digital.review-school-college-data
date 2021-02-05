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

        public IList<AmendmentReason> GetInclusionAdjustmentReasons(CheckingWindow checkingWindow, string pinclId = "")
        {
            IQueryable<AmendmentReason> reasons;

            if (pinclId != string.Empty)
            {
                reasons = _repository.Get<DTO.PinclinclusionAdjustment>()
                    .Where(x => x.PIncl == pinclId)
                    .Select(x => x.IncAdjReason)
                    .Select(x => new AmendmentReason
                    {
                        ReasonId = x.IncAdjReasonId,
                        CanCancel = x.CanCancel,
                        InJune = x.InJuneChecking,
                        Description = x.IncAdjReasonDescription,
                        IsInclusion = x.IsInclusion,
                        IsNew = x.IsNewStudentReason,
                        Order = x.ListOrder
                    }).OrderBy(x => x.Order);
            }
            else
            {
                reasons = _repository
                    .Get<DTO.InclusionAdjustmentReason>()
                    .Select(x => new AmendmentReason
                    {
                        ReasonId = x.IncAdjReasonId,
                        CanCancel = x.CanCancel,
                        InJune = x.InJuneChecking,
                        Description = x.IncAdjReasonDescription,
                        IsInclusion = x.IsInclusion,
                        IsNew = x.IsNewStudentReason,
                        Order = x.ListOrder
                    });
            }

            if (checkingWindow == CheckingWindow.KS4June)
            {
                reasons = reasons.Where(x => x.InJune);
            }

            return reasons.ToList();
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