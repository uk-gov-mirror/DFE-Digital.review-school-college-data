using Dfe.CspdAlpha.Web.Application.Models.Common;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments
{
    public class AmendmentsListViewModel : ContextAwareViewModel
    {
        public CheckDataNavigation CheckDataNavigationModel => new CheckDataNavigation(NavigationItem.Amendments , CheckingWindow);
        public string Urn { get; set; }
        public List<AmendmentListItem> AmendmentList { get; set; }
        public string AmendmentListJson => JsonConvert.SerializeObject(AmendmentList);
    }
}
