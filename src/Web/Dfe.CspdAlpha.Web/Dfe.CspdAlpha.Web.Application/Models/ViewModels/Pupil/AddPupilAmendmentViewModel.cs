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
        public AddPupilViewModel AddPupilViewModel { get; set; }
        public AddReason AddReason
        {
            get
            {
                if (AddPupilViewModel == null)
                {
                    return AddReason.Unknown;
                }

                return string.IsNullOrEmpty(AddPupilViewModel.UPN) ? AddReason.New : AddReason.Existing;
            }
        }
        public List<PriorAttainmentResultViewModel> Results { get; set; }
        public EvidenceOption SelectedEvidenceOption { get; set; }
        public List<EvidenceFile> EvidenceFiles { get; set; }
        //public int MatchedPupilCount { get; set; }
        //public string ExistingMatchedPupil { get; set; }
    }
}
