using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.School;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;

namespace Dfe.CspdAlpha.Web.Application.Application.Interfaces
{
    public interface IEstablishmentService
    {
        string GetSchoolName(string laestab);
        SchoolDetails GetSchoolDetails(string urn);
        SchoolViewModel GetSchoolViewModel(string urn);
    }
}
