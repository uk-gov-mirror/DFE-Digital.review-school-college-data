using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.School;
using Newtonsoft.Json;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels
{
    public class PupilListViewModel
    {
        public CheckDataNavigation CheckDataNavigationModel => new CheckDataNavigation(NavigationItem.PupilList);
        public ConfirmDataBanner ConfirmDataBanner => new ConfirmDataBanner();
        public List<Pupil> Pupils { get; set; }
        public string PupilsJson => JsonConvert.SerializeObject(Pupils);
    }
}
