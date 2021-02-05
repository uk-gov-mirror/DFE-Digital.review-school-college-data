using System;
using System.Collections.Generic;
using Dfe.Rscd.Api.Domain.Entities.Questions;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class Amendment
    {
        public Amendment()
        {
            AmendmentDetail = new AmendmentDetail();
            Answers = new List<UserAnswer>();
        }

        public string URN { get; set; }
        public AmendmentType AmendmentType { get; set; }

        public bool IsUserConfirmed { get;set; }
        public string EvidenceFolderName { get; set; }
        public EvidenceStatus EvidenceStatus { get; set; }
        public string Reference { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
        public string Id { get; set; }
        public CheckingWindow CheckingWindow { get; set; }
        public Pupil Pupil { get; set; }

        public string DfesNumber { get;set; }

        public List<UserAnswer> Answers { get; set; }

        public int InclusionReasonId { get; set; }

        public AmendmentDetail AmendmentDetail { get; set; }
    }


}