using System.Collections.Generic;
using Dfe.Rscd.Web.Application.Models.ViewModels.Pupil;

namespace Dfe.Rscd.Web.Application.Models.ViewModels.Results
{
    public class PriorAttainmentResultViewModel
    {
        public PupilViewModel PupilDetails { get; set; }

        public List<string> Ks2Subjects { get; set; }
        public string Subject { get; set; }
        public string ExamYear { get; set; }
        public string TestMark { get; set; }
        public string ScaledScore { get; set; }
    }
}
