using Dfe.Rscd.Web.Application.Models.ViewModels.Pupil;

namespace Dfe.Rscd.Web.Application.Models.ViewModels.RemovePupil
{
    public class RemovePupilViewModel : ContextAwareViewModel
    {
        public MatchedPupilViewModel MatchedPupilViewModel { get; set; }
        public QueryType QueryType { get; set; }
        public string query { get; set; }
    }
}
