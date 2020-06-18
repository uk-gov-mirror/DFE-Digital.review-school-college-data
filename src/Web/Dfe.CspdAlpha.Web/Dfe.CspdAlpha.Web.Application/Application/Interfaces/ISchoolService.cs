using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;

namespace Dfe.CspdAlpha.Web.Application.Application.Interfaces
{
    public interface ISchoolService
    {
        SchoolViewModel GetSchoolViewModel(string urn);
        PupilListViewModel GetPupilListViewModel(string urn);
    }
}