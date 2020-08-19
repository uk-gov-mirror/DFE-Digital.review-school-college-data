namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil
{
    public class ConfirmAddPupilViewModel
    {
        public PupilViewModel AddPupilViewModel { get; set; }
        public bool ConfirmAddPupil { get; set; }
        public EvidenceOption SelectedEvidenceOption { get; set; }
        public string SelectedEvidenceBackOption =>
            SelectedEvidenceOption == EvidenceOption.UploadNow ? "UploadEvidence" : "AddEvidence";
    }
}
