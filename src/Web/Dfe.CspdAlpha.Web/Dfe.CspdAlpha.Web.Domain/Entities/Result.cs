using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace Dfe.CspdAlpha.Web.Domain.Entities
{
    public class Result
    {
        public string SeasonYear { get; set; }
        public string Qualification { get; set; }
        public string ExamDate { get; set; }
        public string SyllabusCode { get; set; }
        public string AwardingOrganisation { get; set; }
        public string QAN { get; set; }
        public string Subject { get; set; }
        public string NCN { get; set; }
        public string Grade { get; set; }
    }
}
