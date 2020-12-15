using System;
using Dfe.Rscd.Api.Domain.Core.Enums;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class Amendment
    {
        public Amendment()
        {
            AmendmentDetail = new AmendmentDetail();
        }

        public string URN { get; set; }
        public AmendmentType AmendmentType { get; set; }
        public string EvidenceFolderName { get; set; }
        public EvidenceStatus EvidenceStatus { get; set; }
        public string Reference { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
        public string Id { get; set; }
        public CheckingWindow CheckingWindow { get; set; }
        public Pupil Pupil { get; set; }

        public AmendmentDetail AmendmentDetail { get; set; }
    }
}