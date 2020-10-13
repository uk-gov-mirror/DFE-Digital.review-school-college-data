using Dfe.CspdAlpha.Web.Application.Models.Amendments;
using Dfe.CspdAlpha.Web.Application.Models.Common;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments
{
    public class ConfirmViewModel
    {
        public PupilDetails PupilDetails { get; set; }
        public string PupilLabel => PupilDetails?.Keystage == Keystage.KS5 ? "student" : "pupil";
        public AmendmentType AmendmentType { get; set; }
        public bool ConfirmAmendment { get; set; }


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
