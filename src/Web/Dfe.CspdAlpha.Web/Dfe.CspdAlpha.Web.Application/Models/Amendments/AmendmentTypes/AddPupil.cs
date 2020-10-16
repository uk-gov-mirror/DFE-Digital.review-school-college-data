using Dfe.CspdAlpha.Web.Application.Models.Common;
using System.Collections.Generic;

namespace Dfe.CspdAlpha.Web.Application.Models.Amendments.AmendmentTypes
{
    public class AddPupil : IAmendmentType
    {
        public AmendmentType AmendmentType => AmendmentType.AddPupil;
        public AddReason AddReason { get; set; }
        public string PreviousSchoolLAEstab { get; set; }
        public string PreviousSchoolURN { get; set; }
        public List<PriorAttainmentResult> PriorAttainmentResults { get; set; }
    }
}
