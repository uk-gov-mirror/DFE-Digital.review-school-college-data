using Dfe.Rscd.Web.Application.Models.School;
using Dfe.Rscd.Web.Application.Models.ViewModels;

namespace Dfe.Rscd.Web.Application.Application.Interfaces
{
    public interface IEstablishmentService
    {
        string GetSchoolName(string laestab);
        SchoolDetails GetSchoolDetails(string urn);
        SchoolViewModel GetSchoolViewModel(string urn);
    }
}
