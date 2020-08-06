using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Application.Models.Common;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil
{
    public class ConfirmAddPupilViewModel
    {
        public AddReasonViewModel AddReasonViewModel { get; set; }
        public AddPupilViewModel AddPupilViewModel { get; set; }
        public List<School.Pupil> MatchedPupils { get; set; }
        public int MatchedPupilCount { get; set; }
        public string SelectedPupilId { get; set; }
        public bool ConfirmAddPupil { get; set; }
        public EvidenceOption SelectedEvidenceOption { get; set; }
        public string SelectedEvidenceBackOption =>
            SelectedEvidenceOption == EvidenceOption.UploadNow ? "UploadEvidence" : "AddEvidence";


        public string PageTitle()
        {
            if (MatchedPupils == null)
            {
                return AddPupilViewModel.AddReason == AddReason.New
                    ? "Confirm you want to request to add this new pupil"
                    : "Confirm you want to request to add this existing pupil";
            }

            var text = MatchedPupils.Count > 1 ? "matches" : "match";
            return
                $"We found {MatchedPupils.Count} similar {text} for the pupil you want to request to add.";
        }
    }
}
