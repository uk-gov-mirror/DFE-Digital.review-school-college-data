using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using System.Collections.Generic;
using System.Linq;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Results
{
    public class ExistingResultsViewModel
    {
        public ExistingResultsViewModel()
        {

        }

        public ExistingResultsViewModel(List<PriorAttainmentResultViewModel> results, PupilViewModel pupilViewModel)
        {
            PupilViewModel = pupilViewModel;
            Reading = results.SingleOrDefault(r => r.Subject == Ks2Subject.Reading);
            Writing = results.SingleOrDefault(r => r.Subject == Ks2Subject.Writing);
            Maths = results.SingleOrDefault(r => r.Subject == Ks2Subject.Maths);
        }

        public PupilViewModel PupilViewModel { get; set; }

        public PriorAttainmentResultViewModel Reading { get; set; }
        public PriorAttainmentResultViewModel Writing { get; set; }
        public PriorAttainmentResultViewModel Maths { get; set; }
    }
}
