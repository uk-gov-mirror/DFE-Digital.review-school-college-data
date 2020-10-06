using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.School;
using Newtonsoft.Json;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil
{
    public class PupilListViewModel
    {
        public CheckDataNavigation CheckDataNavigationModel => new CheckDataNavigation(NavigationItem.PupilList, CheckingWindow);
        public SchoolDetails SchoolDetails { get; set; }
        public List<PupilDetails> Pupils { get; set; }
        public string PupilsJson => JsonConvert.SerializeObject(Pupils);
        public CheckingWindow CheckingWindow { get; set; }
    }
}
