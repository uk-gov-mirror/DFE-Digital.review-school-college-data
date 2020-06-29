using System;
using System.Collections.Generic;
using System.Text;
using Dfe.CspdAlpha.Web.Domain.Core.Enums;

namespace Dfe.CspdAlpha.Web.Domain.Entities
{
    public class AddPupilAmendment
    {
        public string AddReason { get; set; }
        public Pupil Pupil { get; set; }
        public PriorAttainment PriorAttainment { get; set; }
        public bool InclusionConfirmed { get; set; }
        public EvidenceStatus EvidenceStatus { get; set; }
        public List<Evidence> EvidenceList { get; set; }
    }
}
