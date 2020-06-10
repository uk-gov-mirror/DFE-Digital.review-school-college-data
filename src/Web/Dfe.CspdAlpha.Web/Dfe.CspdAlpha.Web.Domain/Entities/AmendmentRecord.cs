using System;
using System.Collections.Generic;
using System.Text;
using Dfe.CspdAlpha.Web.Domain.Core;

namespace Dfe.CspdAlpha.Web.Domain.Entities
{
    public class AmendmentRecord
    {
        public URN Urn { get; set; }
        public PupilId PupilId { get; set; }

        public List<Evidence> EvidenceList { get; set; }
        public List<Audit> AuditList { get; set; }
    }
}
