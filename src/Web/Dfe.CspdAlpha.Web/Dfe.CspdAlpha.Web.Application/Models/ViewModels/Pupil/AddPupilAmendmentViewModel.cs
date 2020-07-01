using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Dfe.CspdAlpha.Web.Application.Models.Common;

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
        public string LaEstab { get; set; }
        public AddReasonViewModel AddReasonViewModel { get; set; }
        public AddPupilViewModel AddPupilViewModel { get; set; }
        public AddPriorAttainmentViewModel AddPriorAttainmentViewModel { get; set; }
        public EvidenceOption SelectedEvidenceOption { get; set; }
        public List<EvidenceFile> EvidenceFiles { get; set; }
        public bool InclusionConfirmed { get; set; }

        public string SelectedEvidenceBackOption =>
            SelectedEvidenceOption == EvidenceOption.UploadNow ? "UploadEvidence" : "AddEvidence";
    }
}
