using System.Collections.Generic;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Entities.ReferenceData;

namespace Dfe.Rscd.Api.Domain.Interfaces
{
    public interface ICommonDataService
    {
        IList<AmendReference> GetAmendCodes();
        IList<AwardingBody> GetAwardingBodies();
        IList<Ethnicity> GetEthnicities();
        IList<InclusionAdjustmentReason> GetInclusionAdjustmentReasons();
        IList<Language> GetLanguages();
        IList<Pincl> GetPINCLs();
        IList<SenStatus> GetSENStatus();
        IList<YearGroup> GetYearGroups();
    }
}