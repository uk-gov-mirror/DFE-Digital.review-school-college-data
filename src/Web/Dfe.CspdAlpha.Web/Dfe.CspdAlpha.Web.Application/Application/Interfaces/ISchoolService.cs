using Dfe.Rscd.Web.Application.Models.ViewModels;

namespace Dfe.Rscd.Web.Application.Application.Interfaces
{
    public interface ISchoolService
    {
        bool UpdateConfirmation(TaskListViewModel taskListViewModel, string userId, string urn);
        TaskListViewModel GetConfirmationRecord(string userId, string urn);
    }
}
