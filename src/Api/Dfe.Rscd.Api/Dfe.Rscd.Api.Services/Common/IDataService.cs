using System.Collections.Generic;
using System.Threading.Tasks;
using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Services
{
    public interface IDataService
    {
        IList<AmendCode> GetAmendCodes();
        IList<AwardingBody> GetAwardingBodies();
        IList<Ethnicity> GetEthnicities();
        IList<AmendmentReason> GetInclusionAdjustmentReasons(CheckingWindow checkingWindow, string pinclId = "");
        IList<FirstLanguage> GetLanguages();
        IList<PInclude> GetPINCLs();
        IList<SpecialEducationNeed> GetSENStatus();
        IList<YearGroup> GetYearGroups();
    }
}