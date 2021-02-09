using Dfe.Rscd.Web.Application.Models.ViewModels.Pupil;

namespace Dfe.Rscd.Web.Application.Models.ViewModels.AddPupil
{
    public class MatchedAddPupilViewModel : ContextAwareViewModel
    {
        public PupilViewModel PupilViewModel { get; set; }
        public string SchoolName { get; set; }
    }
}
