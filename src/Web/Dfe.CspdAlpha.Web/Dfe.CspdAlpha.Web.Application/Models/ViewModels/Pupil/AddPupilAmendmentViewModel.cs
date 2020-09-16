using Dfe.CspdAlpha.Web.Application.Models.Common;
using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Results;

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
        public PupilViewModel PupilViewModel { get; set; }
        public AddReason AddReason
        {
            get
            {
                if (PupilViewModel == null)
                {
                    return AddReason.Unknown;
                }

                return string.IsNullOrEmpty(PupilViewModel.UPN) ? AddReason.New : AddReason.Existing;
            }
        }
        public List<PriorAttainmentResultViewModel> Results { get; set; }
        public EvidenceOption SelectedEvidenceOption { get; set; }

        public string EvidenceFolderName { get; set; }
    }
}
