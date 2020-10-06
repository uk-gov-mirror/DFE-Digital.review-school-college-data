using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Application.Models.Common;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil
{
    public class ReasonViewModel
    {
        public PupilDetails PupilDetails { get; set; }

        public string SelectedReason { get; set; }

        public Dictionary<string, string> Reasons = new Dictionary<string, string>
        {
            { "325", "Not at the end of 16 to 18 study" },
            { "326", "International student" },
            { "327", "Deceased" },
            { "328", "Not on roll" },
            { "329", "Other - with evidence" },
            { "330", "Other - evidence not required" },
        };
    }
}
