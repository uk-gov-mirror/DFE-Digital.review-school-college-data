using Dfe.CspdAlpha.Web.Application.Models.Common;
using System.Collections.Generic;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil
{
    public class SubReasonViewModel
    {
        public PupilDetails PupilDetails { get; set; }
        public string SelectedReason { get; set; }
        public Dictionary<string, string> Reasons { get; set; }
    }
}
