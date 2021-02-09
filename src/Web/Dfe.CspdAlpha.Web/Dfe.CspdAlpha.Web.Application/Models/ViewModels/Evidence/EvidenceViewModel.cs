using Dfe.Rscd.Web.ApiClient;
using Dfe.Rscd.Web.Application.Models.ViewModels.Pupil;

namespace Dfe.Rscd.Web.Application.Models.ViewModels.Evidence
{
    public class EvidenceViewModel : ContextAwareViewModel
    {
        public PupilViewModel PupilDetails { get; set; }
        public EvidenceStatus EvidenceOption { get; set; }
        public string AddReason { get; set; }
    }
}
