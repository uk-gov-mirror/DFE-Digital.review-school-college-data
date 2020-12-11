using System.Collections.Generic;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Interfaces;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class AddPupilAmendmentDetail : IAmendmentDetail
    {
        public AddReason Reason { get; set; }
        public string PreviousSchoolURN { get; set; }
        public string PreviousSchoolLAEstab { get; set; }
        public List<PriorAttainment> PriorAttainmentResults { get; set; }
        public int ReasonCode { get; set; }
        public string SubReason { get; set; }
        public string Detail { get; set; }
    }
}