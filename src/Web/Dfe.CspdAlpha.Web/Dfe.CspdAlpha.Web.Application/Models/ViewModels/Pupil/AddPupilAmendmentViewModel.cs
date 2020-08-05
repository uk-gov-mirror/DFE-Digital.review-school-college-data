using Dfe.CspdAlpha.Web.Application.Models.Common;
using System.Collections.Generic;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil
{
    public enum EvidenceOption
    {
        Unknown,
        UploadNow,
        UploadLater,
        NotRequired
    }

    public class AddPupilAmendmentViewModel
    {
        public string URN { get; set; }
        public AddReasonViewModel AddReasonViewModel { get; set; }
        public string ExistingMatchedPupil { get; set; }
        public AddPupilViewModel AddPupilViewModel { get; set; }
        public AddPriorAttainmentViewModel AddPriorAttainmentViewModel { get; set; }
        public EvidenceOption SelectedEvidenceOption { get; set; }
        public List<EvidenceFile> EvidenceFiles { get; set; }
        public bool InclusionConfirmed { get; set; }

        public string SelectedEvidenceBackOption =>
            SelectedEvidenceOption == EvidenceOption.UploadNow ? "UploadEvidence" : "AddEvidence";
    }
}
