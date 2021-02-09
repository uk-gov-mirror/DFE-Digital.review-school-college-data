using System.Collections.Generic;
using Dfe.Rscd.Web.Application.Models.Common;
using Newtonsoft.Json;

namespace Dfe.Rscd.Web.Application.Models.ViewModels.Amendments
{
    public class AmendmentsListViewModel : ContextAwareViewModel
    {
        public CheckDataNavigation CheckDataNavigationModel => new CheckDataNavigation(NavigationItem.Amendments , CheckingWindow);
        public string Urn { get; set; }
        public List<AmendmentListItem> AmendmentList { get; set; }
        public string AmendmentListJson => JsonConvert.SerializeObject(AmendmentList);
    }
}
