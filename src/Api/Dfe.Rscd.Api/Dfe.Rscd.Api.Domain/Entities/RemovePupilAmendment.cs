using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Interfaces;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class RemovePupilAmendment : Amendment
    {
        public RemovePupilAmendment()
        {
            AmendmentType = AmendmentType.RemovePupil;
        }

        public Pupil Pupil { get; set; }
        public int ScrutinyReasonCode { get; set; }
        public string AmdFlag { get; set; }

        public override IAmendmentDetail GetAmendmentDetail()
        {
            return AmendmentDetail ?? (AmendmentDetail = new AmendmentDetail());
        }
    }
}