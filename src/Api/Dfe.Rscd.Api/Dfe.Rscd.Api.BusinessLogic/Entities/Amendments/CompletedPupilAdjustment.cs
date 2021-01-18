using System.Collections.Generic;

namespace Dfe.Rscd.Api.BusinessLogic.Entities
{
	public partial class CompletedPupilAdjustment 
	{
        public short? InclusionReasonID { get; set; }

        public int PupilID { get; set; }

        public List<PromptAnswer> PromptAnswerList { get; set; }

        public short ScrutinyReasonID { get; set; }

        public short? RejectionReasonCode { get; set; }

        public string ScrutinyStatusCode { get; set; }

        public string RequestCompletionDisplayMessage { get; set; }

        public int? StudentRequestID { get; set; }

        public string ScrutinyStatusDescription { get; set; }

        public int ForvusId { get; set; }
    }
}

