using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Common;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil
{
    public class AddPupilDetailsViewModel
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
