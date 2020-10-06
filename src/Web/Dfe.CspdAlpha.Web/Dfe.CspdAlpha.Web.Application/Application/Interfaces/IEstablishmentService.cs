using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.School;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;

namespace Dfe.CspdAlpha.Web.Application.Application.Interfaces
{
    public interface IEstablishmentService
    {
        string GetSchoolName(CheckingWindow checkingWindow, string laestab);
        SchoolDetails GetSchoolDetails(CheckingWindow checkingWindow, string urn);
        SchoolViewModel GetSchoolViewModel(CheckingWindow checkingWindow, string urn);
    }
}
