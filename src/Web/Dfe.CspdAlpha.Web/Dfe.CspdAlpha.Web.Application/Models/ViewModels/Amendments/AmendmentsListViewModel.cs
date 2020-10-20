using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.School;
using Newtonsoft.Json;
using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Application.Models.Amendments;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments
{
    public class AmendmentsListViewModel
    {
        public CheckDataNavigation CheckDataNavigationModel => new CheckDataNavigation(NavigationItem.Amendments , CheckingWindow);
        public string Urn { get; set; }
        public List<AmendmentListItem> AmendmentList { get; set; }
        public CheckingWindow CheckingWindow { get; set; }
        public string AmendmentListJson => JsonConvert.SerializeObject(AmendmentList);
    }
}
