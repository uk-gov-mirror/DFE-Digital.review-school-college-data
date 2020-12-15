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
            Reading = results.SingleOrDefault(r => r.Ks2Subject == Ks2Subject.Reading);
            Writing = results.SingleOrDefault(r => r.Ks2Subject == Ks2Subject.Writing);
            Maths = results.SingleOrDefault(r => r.Ks2Subject == Ks2Subject.Maths);
        }

        public PupilViewModel PupilDetails { get; set; }
        public PriorAttainmentResult Reading { get; set; }
        public PriorAttainmentResult Writing { get; set; }
        public PriorAttainmentResult Maths { get; set; }
    }
}
