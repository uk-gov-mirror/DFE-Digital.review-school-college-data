namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil
{
    public class ConfirmAddPupilViewModel
    {
        public PupilViewModel PupilViewModel { get; set; }
        public bool ConfirmAddPupil { get; set; }
        public EvidenceOption SelectedEvidenceOption { get; set; }
        public string SelectedEvidenceBackOption =>
            SelectedEvidenceOption == EvidenceOption.UploadNow ? "UploadEvidence" : "AddEvidence";
    }
}
