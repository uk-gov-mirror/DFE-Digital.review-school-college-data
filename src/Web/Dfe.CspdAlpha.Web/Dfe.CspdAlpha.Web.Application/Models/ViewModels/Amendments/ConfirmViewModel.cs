using Dfe.Rscd.Web.ApiClient;
using Dfe.Rscd.Web.Application.Models.Common;
using Dfe.Rscd.Web.Application.Models.ViewModels.Pupil;

namespace Dfe.Rscd.Web.Application.Models.ViewModels.Amendments
{
    public class ConfirmViewModel : ContextAwareViewModel
    {
        public PupilViewModel PupilDetails { get; set; }
        public string PupilLabel => GetPupilLabel();
        public AmendmentType AmendmentType { get; set; }
        public bool ConfirmAmendment { get; set; }

        public string BackController { get; set; }
        public string BackAction { get; set; }

        private string GetPupilLabel()
        {
            if (PupilDetails != null)
            {
                return PupilDetails.Keystage == Keystage.KS5 ? "student" : "pupil";
            }

            return string.Empty;
        }


        public string GetTitle()
        {
            var pupilLabel = PupilDetails.Keystage == Keystage.KS5 ? "student" : "pupil";
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
