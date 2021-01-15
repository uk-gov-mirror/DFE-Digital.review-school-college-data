using System.Collections.Generic;
using Dfe.Rscd.Api.BusinessLogic.Contracts.Entities;
using Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs;
using AwardingBody = Dfe.Rscd.Api.BusinessLogic.Contracts.Entities.AwardingBody;
using Ethnicity = Dfe.Rscd.Api.BusinessLogic.Contracts.Entities.Ethnicity;

namespace Dfe.Rscd.Api.BusinessLogic.Contracts.Services
{
    public interface IDataService
    {
        IEnumerable<T> Query<T>() where T : class;
        IList<AmendCode> GetAmendCodes();
        IList<AwardingBody> GetAwardingBodies();
        IList<Ethnicity> GetEthnicities();
        IList<InclusionAdjustmentReason> GetInclusionAdjustmentReasons();
        IList<FirstLanguage> GetLanguages();
        IList<PINCLs> GetPINCLs();
        IList<SENStatus> GetSENStatus();
        IList<YearGroup> GetYearGroups();
    }
}