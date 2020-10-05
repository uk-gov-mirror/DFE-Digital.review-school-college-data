using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;

namespace Dfe.CspdAlpha.Web.Application.Application.Interfaces
{
    public interface ISchoolService
    {
        PupilListViewModel GetPupilListViewModel(CheckingWindow checkingWindow, string urn);
        PupilListViewModel GetPupilListViewModel(CheckingWindow checkingWindow, string urn, string id, string name);
        MatchedPupilViewModel GetPupil(CheckingWindow checkingWindow, string id);
        MatchedPupilViewModel GetMatchedPupil(CheckingWindow checkingWindow, string upn);

        bool UpdateConfirmation(TaskListViewModel taskListViewModel, string userId, string urn);
        TaskListViewModel GetConfirmationRecord(CheckingWindow checkingWindow, string userId, string urn);
    }
}
