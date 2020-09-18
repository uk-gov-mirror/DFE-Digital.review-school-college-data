using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;

namespace Dfe.CspdAlpha.Web.Application.Application.Interfaces
{
    public interface ISchoolService
    {
        string GetSchoolName(string laestab);
        SchoolViewModel GetSchoolViewModel(string urn);
        PupilListViewModel GetPupilListViewModel(string urn);

        MatchedPupilViewModel GetPupil(string id);
        MatchedPupilViewModel GetMatchedPupil(string upn);

        bool UpdateConfirmation(TaskListViewModel taskListViewModel, string userId, string urn);
        TaskListViewModel GetConfirmationRecord(string userId, string urn);
    }
}
