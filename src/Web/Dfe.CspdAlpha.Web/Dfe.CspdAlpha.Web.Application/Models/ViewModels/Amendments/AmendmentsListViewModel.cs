using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.School;
using Dfe.CspdAlpha.Web.Domain.Entities;
using Newtonsoft.Json;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments
{
    public class AmendmentsListViewModel
    {
        public CheckDataNavigation CheckDataNavigationModel => new CheckDataNavigation(NavigationItem.Amendments);
        public string Urn { get; set; }
        public List<Amendment> AmendmentList { get; set; }
        public string AmendmentListJson => JsonConvert.SerializeObject(AmendmentList);
    }
}
