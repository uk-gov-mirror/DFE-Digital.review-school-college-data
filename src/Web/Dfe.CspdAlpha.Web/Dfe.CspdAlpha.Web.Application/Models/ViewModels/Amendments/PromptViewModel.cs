using System.Collections.Generic;
using System.Linq;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Dfe.Rscd.Web.ApiClient;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments
{
    public class PromptViewModel
    {
        public string GetTitle()
        {
            return "Further Questions";
        }

        public PupilViewModel PupilDetails { get; set; }
        public string PupilLabel => GetPupilLabel();
        public AmendmentType AmendmentType { get; set; }

        public List<Prompt> Questions { get;set; }

        private string GetPupilLabel()
        {
            if (PupilDetails != null)
            {
                return PupilDetails.Keystage == Keystage.KS5 ? "student" : "pupil";
            }

            return string.Empty;
        }
        public string BackController { get; set; }
        public string BackAction { get; set; }

        private readonly AmendmentOutcome _outcome;

        public PromptViewModel(AmendmentOutcome outcome)
        {
            _outcome = outcome;
            Questions = outcome.FurtherPrompts.ToList();
        }
    }
}