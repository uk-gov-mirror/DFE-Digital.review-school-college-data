using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Results
{
    public class PriorAttainmentResultViewModel
    {
        public PupilViewModel PupilDetails { get; set; }

        public List<Ks2Subject> Ks2Subjects { get; set; }
        public Ks2Subject Subject { get; set; }
        public string ExamYear { get; set; }
        public string TestMark { get; set; }
        public string ScaledScore { get; set; }
    }
}
