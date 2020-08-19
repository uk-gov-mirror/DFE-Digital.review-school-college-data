using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Newtonsoft.Json;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil
{
    public class PupilListViewModel
    {
        public CheckDataNavigation CheckDataNavigationModel => new CheckDataNavigation(NavigationItem.PupilList);
        public string Urn { get; set; }
        public List<PupilListEntry> Pupils { get; set; }
        public string PupilsJson => JsonConvert.SerializeObject(Pupils);
        public bool LateCheckingPhase { get; set; }
    }

    public class PupilListEntry
    {
        public string PupilId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
