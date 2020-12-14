using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Common;
using Dfe.Rscd.Web.ApiClient;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.AddPupil
{
    public class AddPupilViewModel
    {
        public string SchoolID { get; set; }

        public string UPN { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateViewModel DateOfBirth { get; set; }

        public Gender? Gender { get; set; }

        public DateViewModel DateOfAdmission { get; set; }

        public string YearGroup { get; set; }
    }
}
