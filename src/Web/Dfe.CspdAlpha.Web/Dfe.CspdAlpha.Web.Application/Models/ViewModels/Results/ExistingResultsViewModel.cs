using System.Collections.Generic;
using System.Linq;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Results
{
    public class ExistingResultsViewModel
    {
        public ExistingResultsViewModel()
        {
            // Required for model binding
        }

        public ExistingResultsViewModel(List<PriorAttainmentResult> results, PupilViewModel pupilDetails)
        {
            PupilDetails = pupilDetails;
            Reading = results.FirstOrDefault(r => r.Ks2Subject == Ks2Subject.Reading && r.Mark != string.Empty);
            Writing = results.FirstOrDefault(r => r.Ks2Subject == Ks2Subject.Writing && r.Mark != string.Empty);
            Maths = results.FirstOrDefault(r => r.Ks2Subject == Ks2Subject.Maths && r.Mark != string.Empty);
        }

        public PupilViewModel PupilDetails { get; set; }
        public PriorAttainmentResult Reading { get; set; }
        public PriorAttainmentResult Writing { get; set; }
        public PriorAttainmentResult Maths { get; set; }
    }
}
