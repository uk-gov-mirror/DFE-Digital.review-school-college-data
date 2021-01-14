using System.Collections.Generic;
using Dfe.Rscd.Api.Domain.Entities.ReferenceData;
using Dfe.Rscd.Api.Domain.Interfaces;
using Dfe.Rscd.Api.Infrastructure.SqlServer.Repositories;

namespace Dfe.Rscd.Api.Infrastructure.SqlServer.Services
{
    public class CommonDataService : ICommonDataService
    {
        private readonly ICommonData _repository;

        public CommonDataService(ICommonData repository)
        {
            _repository = repository;
        }

        public IList<AmendReference> GetAmendCodes()
        {
            return _repository.Get<AmendReference>("AmendCodes");
        }

        public IList<AwardingBody> GetAwardingBodies()
        {
            return _repository.Get<AwardingBody>("AwardingBodies");
        }

        public IList<Ethnicity> GetEthnicities()
        {
            return _repository.Get<Ethnicity>("Ethnicities");
        }

        public IList<InclusionAdjustmentReason> GetInclusionAdjustmentReasons()
        {
            return _repository.Get<InclusionAdjustmentReason>("InclusionAdjustmentReasons");
        }

        public IList<Language> GetLanguages()
        {
            return _repository.Get<Language>("Languages");
        }

        public IList<Pincl> GetPINCLs()
        {
            return _repository.Get<Pincl>("PINCLs");
        }

        public IList<SenStatus> GetSENStatus()
        {
            return _repository.Get<SenStatus>("SENStatus");
        }

        public IList<YearGroup> GetYearGroups()
        {
            return _repository.Get<YearGroup>("YearGroups");
        }
    }
}