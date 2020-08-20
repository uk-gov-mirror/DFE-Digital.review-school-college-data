using System;
using System.Collections.Generic;
using System.Text;
using Dfe.CspdAlpha.Web.Domain.Core.Enums;

namespace Dfe.CspdAlpha.Web.Domain.Entities
{
    public class PriorAttainment
    {
        public Ks2Subject Subject { get; set; }
        public string ExamYear { get; set; }
        public string TestMark { get; set; }
        public string ScaledScore { get; set; }
    }
}
