using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Dfe.Rscd.Web.ApiClient;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil
{
    public class ReasonViewModel
    {
        public QueryType SearchType { get; set; }

        public string Query { get; set; }

        public string MatchedId { get; set; }

        public PupilViewModel PupilDetails { get; set; }

        public int? SelectedReasonCode { get; set; }

        public List<InclusionAdjustmentReason> Reasons = new List<InclusionAdjustmentReason>();
    }
}
