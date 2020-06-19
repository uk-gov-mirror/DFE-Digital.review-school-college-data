using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public AddPupilViewModel AddPupilViewModel { get; set; }
        public AddPriorAttainmentViewModel AddPriorAttainmentViewModel { get; set; }
        public EvidenceOption SelectedEvidenceOption { get; set; }

        public List<string> EvidenceFiles { get; set; }
    }
}
