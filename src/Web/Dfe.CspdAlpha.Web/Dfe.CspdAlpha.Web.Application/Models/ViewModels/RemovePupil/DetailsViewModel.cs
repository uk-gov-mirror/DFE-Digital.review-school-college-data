using Dfe.Rscd.Web.Application.Models.ViewModels.Pupil;

namespace Dfe.Rscd.Web.Application.Models.ViewModels.RemovePupil
{
    public class DetailsViewModel : ContextAwareViewModel
    {
        public PupilViewModel PupilDetails { get; set; }
        public string AmendmentDetails { get; set; }

    }
}
