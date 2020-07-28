using System;
using System.Collections.Generic;
using System.Text;
using Dfe.CspdAlpha.Web.Domain.Core.Enums;

namespace Dfe.CspdAlpha.Web.Domain.Entities
{
    public class AddPupilAmendment
    {
        public string Id { get; set; }
        public string Reference  { get; set; }
        public AddReason AddReason { get; set; }
        public bool ExistingPupilFound { get; set; }
        public Pupil Pupil { get; set; }
        public PriorAttainment PriorAttainment { get; set; }
        public bool InclusionConfirmed { get; set; }
        public EvidenceStatus EvidenceStatus { get; set; }
        public List<Evidence> EvidenceList { get; set; }

        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
