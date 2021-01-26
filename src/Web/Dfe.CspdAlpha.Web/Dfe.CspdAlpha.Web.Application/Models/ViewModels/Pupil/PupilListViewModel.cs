using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.School;
using Newtonsoft.Json;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil
{
    public class PupilListViewModel : ContextAwareViewModel
    {
        public CheckDataNavigation CheckDataNavigationModel => new CheckDataNavigation(NavigationItem.PupilList, CheckingWindow);
        public SchoolDetails SchoolDetails { get; set; }
        public List<PupilViewModel> Pupils { get; set; }
        public string PupilsJson => JsonConvert.SerializeObject(Pupils);
    }
}
