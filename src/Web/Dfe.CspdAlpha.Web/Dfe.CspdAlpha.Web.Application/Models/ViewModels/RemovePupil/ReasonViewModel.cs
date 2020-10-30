using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Application.Models.Common;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil
{
    public class ReasonViewModel
    {
        public QueryType SearchType { get; set; }
        public string Query { get; set; }
        public string MatchedId { get; set; }
        public PupilDetails PupilDetails { get; set; }
        public string SelectedReason { get; set; }
        public Dictionary<string, string> Reasons  => new Dictionary<string, string>
        {
            { Constants.NOT_AT_END_OF_16_TO_18_STUDY, "Not at the end of 16 to 18 study" },
            { Constants.INTERNATIONAL_STUDENT, "International student" },
            { Constants.DECEASED, "Deceased" },
            { Constants.NOT_ON_ROLL, "Not on roll" },
            { Constants.OTHER_WITH_EVIDENCE, "Other with evidence" },
            { Constants.OTHER_EVIDENCE_NOT_REQUIRED, "Other without evidence" },
        };
    }
}
