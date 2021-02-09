using System.Collections.Generic;
using Dfe.Rscd.Web.Application.Models.Common;
using Dfe.Rscd.Web.Application.Models.School;
using Newtonsoft.Json;

namespace Dfe.Rscd.Web.Application.Models.ViewModels.Pupil
{
    public class PupilListViewModel : ContextAwareViewModel
    {
        public CheckDataNavigation CheckDataNavigationModel => new CheckDataNavigation(NavigationItem.PupilList, CheckingWindow);
        public SchoolDetails SchoolDetails { get; set; }
        public List<PupilViewModel> Pupils { get; set; }
        public string PupilsJson => JsonConvert.SerializeObject(Pupils);
    }
}
