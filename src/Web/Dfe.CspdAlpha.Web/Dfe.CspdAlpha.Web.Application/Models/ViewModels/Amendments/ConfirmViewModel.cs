using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.Rscd.Web.ApiClient;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments
{
    public class ConfirmViewModel
    {
        public PupilDetails PupilDetails { get; set; }
        public string PupilLabel => PupilDetails?.KeyStage == Keystage.KS5 ? "student" : "pupil";
        public AmendmentType AmendmentType { get; set; }
        public bool ConfirmAmendment { get; set; }

        public string BackController { get; set; }
        public string BackAction { get; set; }


        public string GetTitle()
        {
            var pupilLabel = PupilDetails.KeyStage == Keystage.KS5 ? "student" : "pupil";
            switch (AmendmentType)
            {
                case AmendmentType.RemovePupil:
                    return $"Confirm you want to request to remove a {PupilLabel}";
                case AmendmentType.AddPupil:
                    return $"Confirm you want to request to add a {PupilLabel}";
            }

            return string.Empty;
        }
    }
}
