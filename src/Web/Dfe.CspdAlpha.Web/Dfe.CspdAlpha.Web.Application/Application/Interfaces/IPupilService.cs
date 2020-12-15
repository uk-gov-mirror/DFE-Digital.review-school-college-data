using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil;
using Dfe.Rscd.Web.ApiClient;

namespace Dfe.CspdAlpha.Web.Application.Application.Interfaces
{
    public interface IPupilService
    {
        List<PupilViewModel> GetPupilDetailsList(CheckingWindow checkingWindow, SearchQuery searchQuery);
        MatchedPupilViewModel GetPupil(CheckingWindow checkingWindow, string id);
        MatchedPupilViewModel GetMatchedPupil(CheckingWindow checkingWindow, string upn);
    }
}
