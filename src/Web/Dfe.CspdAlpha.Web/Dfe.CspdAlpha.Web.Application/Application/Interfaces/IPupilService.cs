using System.Collections.Generic;
using Dfe.Rscd.Web.ApiClient;
using Dfe.Rscd.Web.Application.Models.ViewModels.Pupil;
using Dfe.Rscd.Web.Application.Models.ViewModels.RemovePupil;

namespace Dfe.Rscd.Web.Application.Application.Interfaces
{
    public interface IPupilService
    {
        List<PupilViewModel> GetPupilDetailsList(SearchQuery searchQuery);
        MatchedPupilViewModel GetPupil(string id);
        MatchedPupilViewModel GetMatchedPupil(string upn);

        List<AmendmentReason> GetAmendmentReasons(AmendmentType amendmentType);
    }
}
