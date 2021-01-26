using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil;
using Dfe.Rscd.Web.ApiClient;

namespace Dfe.CspdAlpha.Web.Application.Application.Interfaces
{
    public interface IPupilService
    {
        List<PupilViewModel> GetPupilDetailsList(SearchQuery searchQuery);
        MatchedPupilViewModel GetPupil(string id);
        MatchedPupilViewModel GetMatchedPupil(string upn);

        List<InclusionAdjustmentReason> GetInclusionAdjustmentReasons(string pinclId);
    }
}
