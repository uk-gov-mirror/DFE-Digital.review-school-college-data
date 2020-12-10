namespace Dfe.CspdAlpha.Web.Application.Models.Amendments.AmendmentTypes
{
    public class RemovePupil : IAmendmentType
    {
        public AmendmentType AmendmentType => AmendmentType.RemovePupil;

        public int ReasonCode { get; set; }

        public string SubReason { get; set; }

        public string Detail { get; set; }

        public int[] AmmendmentYears { get; set; }
    }
}
