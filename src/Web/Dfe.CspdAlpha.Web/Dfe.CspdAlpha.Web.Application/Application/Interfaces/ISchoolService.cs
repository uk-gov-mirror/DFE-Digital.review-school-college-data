using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil;

namespace Dfe.CspdAlpha.Web.Application.Application.Interfaces
{
    public interface ISchoolService
    {
        PupilListViewModel GetPupilListViewModel(CheckingWindow checkingWindow, SearchQuery searchQuery);
        MatchedPupilViewModel GetPupil(CheckingWindow checkingWindow, string id);
        MatchedPupilViewModel GetMatchedPupil(CheckingWindow checkingWindow, string upn);

        bool UpdateConfirmation(TaskListViewModel taskListViewModel, string userId, string urn);
        TaskListViewModel GetConfirmationRecord(CheckingWindow checkingWindow, string userId, string urn);
    }
}
