using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.Rscd.Web.ApiClient;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Evidence
{
    public class EvidenceViewModel
    {
        public PupilDetails PupilDetails { get; set; }
        public EvidenceStatus EvidenceOption { get; set; }
        public string AddReason { get; set; }
    }
}
