using System;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Interfaces;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class Amendment
    {
        public string Id { get; set; }
        public CheckingWindow CheckingWindow { get; set; }
        public AmendmentType AmendmentType { get; set; }
        public string Reference { get; set; }
        public string URN { get; set; }
        public Pupil Pupil { get; set; }
        public string EvidenceFolderName { get; set; }
        public EvidenceStatus EvidenceStatus { get; set; }
        public IAmendmentType AmendmentDetail { get; set; }   
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }

        public int ScrutinyReasonCode { get; set; }
        public string AmdFlag { get; set; }
    }
}
