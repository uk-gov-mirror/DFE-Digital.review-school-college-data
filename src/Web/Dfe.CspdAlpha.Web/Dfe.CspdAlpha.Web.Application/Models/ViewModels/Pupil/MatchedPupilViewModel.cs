using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Results;
using System.Collections.Generic;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil
{
    public class MatchedPupilViewModel : ContextAwareViewModel
    {
        public PupilViewModel PupilViewModel { get; set; }
        public List<PriorAttainmentResultViewModel> Results { get; set; }
    }
}
