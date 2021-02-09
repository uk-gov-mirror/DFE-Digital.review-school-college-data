using Dfe.Rscd.Web.Application.Models.ViewModels.Common;

namespace Dfe.Rscd.Web.Application.Models.ViewModels.AddPupil
{
    public class AddPupilViewModel : ContextAwareViewModel
    {

        public AddPupilViewModel(string phase) : base(phase)
        {
            
        }

        public AddPupilViewModel() : base()
        {
            
        }

        public string SchoolID { get; set; }

        public string UPN { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateViewModel DateOfBirth { get; set; }

        public string Gender { get; set; }

        public DateViewModel DateOfAdmission { get; set; }

        public string YearGroup { get; set; }
    }
}
