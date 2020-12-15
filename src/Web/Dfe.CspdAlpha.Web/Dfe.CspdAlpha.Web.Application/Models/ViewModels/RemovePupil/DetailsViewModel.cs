using System.Linq;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil
{
    public class DetailsViewModel
    {
        public PupilViewModel PupilDetails { get; set; }
        public string AmendmentDetails { get; set; }

    }
}
