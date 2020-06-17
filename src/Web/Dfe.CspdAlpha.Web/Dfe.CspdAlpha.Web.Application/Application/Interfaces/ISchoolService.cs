using Dfe.CspdAlpha.Web.Application.Models.ViewModels;

namespace Dfe.CspdAlpha.Web.Application.Application.Interfaces
{
    public interface ISchoolService
    {
        SchoolViewModel GetSchoolViewModel(string urn);
        PupilListViewModel GetPupilListViewModel(string urn);
    }
}