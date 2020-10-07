using Dfe.CspdAlpha.Web.Application.Models.Amendments;
using Dfe.CspdAlpha.Web.Application.Models.Common;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments
{
    public class ConfirmViewModel
    {
        public PupilDetails PupilDetails { get; set; }
        public AmendmentType AmendmentType { get; set; }
        public bool ConfirmAmendment { get; set; }


        public string GetTitle()
        {
            switch (AmendmentType)
            {
                case AmendmentType.RemovePupil:
                    return "Confirm you want to request to remove a student";
                case AmendmentType.AddPupil:
                    return "Confirm you want to request to add a pupil";
            }

            return string.Empty;
        }
    }
}
