using System.Collections.Generic;
using Dfe.Rscd.Web.ApiClient;
using Dfe.Rscd.Web.Application.Models.Common;
using Dfe.Rscd.Web.Application.Models.ViewModels.Pupil;

namespace Dfe.Rscd.Web.Application.Models.ViewModels.RemovePupil
{
    public class ReasonViewModel : ContextAwareViewModel
    {
        public QueryType SearchType { get; set; }

        public string Query { get; set; }

        public string MatchedId { get; set; }

        public PupilViewModel PupilDetails { get; set; }

        public int? SelectedReasonCode { get; set; }

        public List<AmendmentReason> Reasons = new List<AmendmentReason>();
    }
}
