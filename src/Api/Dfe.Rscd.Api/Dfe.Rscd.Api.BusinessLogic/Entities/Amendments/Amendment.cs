using System;
using System.Collections.Generic;
using Dfe.Rscd.Api.BusinessLogic.Entities.Amendments;

namespace Dfe.Rscd.Api.BusinessLogic.Entities
{
    public class Amendment
    {
        public Amendment()
        {
            AmendmentDetail = new AmendmentDetail();
            Answers = new List<PromptAnswer>();
            IsNewAmendment = true;
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

        public List<PromptAnswer> Answers { get; set; }

        public int InclusionReasonId { get; set; }

        public bool IsNewAmendment { get; set; }

        public AmendmentDetail AmendmentDetail { get; set; }
    }


}