using System.Collections.Generic;
using Dfe.Rscd.Web.ApiClient;
using Dfe.Rscd.Web.Application.Models.ViewModels.Results;

namespace Dfe.Rscd.Web.Application.Models.ViewModels.Pupil
{
    public class AddPupilAmendmentViewModel : ContextAwareViewModel
    {
        public string URN { get; set; }
        public PupilViewModel PupilViewModel { get; set; }
        public string AddReason
        {
            get
            {
                if (PupilViewModel == null)
                {
                    return Dfe.Rscd.Web.ApiClient.AddReason.Unknown;
                }

                return string.IsNullOrEmpty(PupilViewModel.UPN) ? Rscd.Web.ApiClient.AddReason.New : Rscd.Web.ApiClient.AddReason.Existing;
            }
        }
        public List<PriorAttainmentResultViewModel> Results { get; set; }
        public EvidenceStatus SelectedEvidenceOption { get; set; }

        public string EvidenceFolderName { get; set; }
    }
}
