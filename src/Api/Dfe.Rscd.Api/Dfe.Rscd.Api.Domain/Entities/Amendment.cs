using System;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Interfaces;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class Amendment : IAmendment
    {
        public string URN { get; set; }
        public AmendmentType AmendmentType { get; protected set; }
        public string EvidenceFolderName { get; set; }
        public EvidenceStatus EvidenceStatus { get; set; }
        public IAmendmentDetail AmendmentDetail { get; set; }
        public string Reference { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
        public string Id { get; set; }
        public CheckingWindow CheckingWindow { get; set; }
        public Pupil Pupil { get; set; }

        public virtual IAmendmentDetail GetAmendmentDetail()
        {
            return AmendmentDetail ?? new AmendmentDetail();
        }
    }
}