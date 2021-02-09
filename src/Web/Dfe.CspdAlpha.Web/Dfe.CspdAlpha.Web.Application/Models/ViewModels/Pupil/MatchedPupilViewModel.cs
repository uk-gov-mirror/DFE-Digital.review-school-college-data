using System.Collections.Generic;
using Dfe.Rscd.Web.Application.Models.ViewModels.Results;

namespace Dfe.Rscd.Web.Application.Models.ViewModels.Pupil
{
    public class MatchedPupilViewModel : ContextAwareViewModel
    {
        public PupilViewModel PupilViewModel { get; set; }
        public List<PriorAttainmentResultViewModel> Results { get; set; }
    }
}
