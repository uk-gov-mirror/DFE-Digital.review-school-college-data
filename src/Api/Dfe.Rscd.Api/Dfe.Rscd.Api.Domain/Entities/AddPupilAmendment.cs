using System;
using System.Collections.Generic;
using Dfe.Rscd.Api.Domain.Core.Enums;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class AddPupilAmendment
    {
        public string Id { get; set; }
        public string Reference { get; set; }
        public AddReason AddReason { get; set; }
        public Pupil Pupil { get; set; }
        public List<PriorAttainment> PriorAttainmentResults { get; set; }
        public bool InclusionConfirmed { get; set; }
        public EvidenceStatus EvidenceStatus { get; set; }
        public List<Evidence> EvidenceList { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
