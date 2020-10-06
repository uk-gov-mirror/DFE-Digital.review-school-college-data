using Dfe.CspdAlpha.Web.Application.Models.Common;

namespace Dfe.CspdAlpha.Web.Application.Models.Amendments.AmendmentTypes
{
    public class RemovePupil : IAmendmentType
    {
        public AmendmentType AmendmentType => AmendmentType.RemovePupil;
        public PupilDetails PupilDetails { get; set; }
        public string Reason { get; set; }
        public string SubReason { get; set; }
    }
}
