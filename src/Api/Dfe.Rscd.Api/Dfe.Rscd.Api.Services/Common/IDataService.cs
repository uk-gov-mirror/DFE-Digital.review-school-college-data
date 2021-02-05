using System.Collections.Generic;
using System.Threading.Tasks;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Entities.Questions;

namespace Dfe.Rscd.Api.Services
{
    public interface IDataService
    {
        IList<AmendCode> GetAmendCodes();
        IList<AwardingBody> GetAwardingBodies();
        IList<Ethnicity> GetEthnicities();
        IList<FirstLanguage> GetLanguages();
        IList<AmendmentReason> GetInclusionAdjustmentReasons(CheckingWindow checkingWindow, string pinclId = "");
        IList<AnswerPotential> GetAnswerPotentials(string questionId);
        IList<PInclude> GetPINCLs();
        IList<SpecialEducationNeed> GetSENStatus();
        IList<YearGroup> GetYearGroups();
    }
}