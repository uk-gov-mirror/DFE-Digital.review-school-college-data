using Dfe.Rscd.Web.Application.Models.ViewModels.Pupil;

namespace Dfe.Rscd.Web.Application.Models.ViewModels.Amendments
{
    public class CustomMessageViewModel : ContextAwareViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public PupilViewModel PupilDetails { get; set; }
    }
}
