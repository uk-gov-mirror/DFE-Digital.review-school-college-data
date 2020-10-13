using Dfe.CspdAlpha.Web.Application.Models.Common;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil
{
    public class DetailsViewModel
    {
        public PupilDetails PupilDetails { get; set; }
        public string Reason { get; set; }
        public string DetailsHeader => GetDetailsHeader();
        public string AmendmentDetails { get; set; }

        private string GetDetailsHeader()
        {
            if (Reason == "328")
            {
                return "Provide details fo why the student was not on roll during the attendance year";
            }

            return "Provide details";
        }
    }
}
