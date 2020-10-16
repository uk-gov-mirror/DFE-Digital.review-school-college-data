using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;

namespace Dfe.CspdAlpha.Web.Application.Application.Interfaces
{
    public interface ISchoolService
    {
        bool UpdateConfirmation(CheckingWindow checkingWindow, TaskListViewModel taskListViewModel, string userId, string urn);
        TaskListViewModel GetConfirmationRecord(CheckingWindow checkingWindow, string userId, string urn);
    }
}
