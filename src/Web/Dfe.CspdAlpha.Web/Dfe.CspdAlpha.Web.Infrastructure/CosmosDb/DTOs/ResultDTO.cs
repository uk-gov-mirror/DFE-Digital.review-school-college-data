using System;
using System.Collections.Generic;
using System.Text;

namespace Dfe.CspdAlpha.Web.Infrastructure.CosmosDb.DTOs
{
    public class ResultDTO
    {
        public string SubjectCode { get; set; }
        public string ExamYear { get; set; }
        public string TestMark { get; set; }
        public string ScaledScore { get; set; }
    }
}
