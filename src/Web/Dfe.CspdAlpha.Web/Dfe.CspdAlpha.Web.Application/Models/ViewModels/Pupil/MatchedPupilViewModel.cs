using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Results;
using System.Collections.Generic;
using Microsoft.Crm.Sdk.Messages;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil
{
    public class MatchedPupilViewModel
    {
        public PupilViewModel PupilViewModel { get; set; }
        public List<PriorAttainmentResultViewModel> Results { get; set; }

        public static string RenderValue(string resultValue)
        {
            return string.IsNullOrWhiteSpace(resultValue) ? "Not recorded" : resultValue;
        }
    }
}
