using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil
{
    public class ResultsViewModel
    {
        public PupilListViewModel PupilListViewModel { get; set; }

        public string PageTitle => PupilListViewModel.Pupils.Count == 0
            ? "No matches found"
            : "We found multiple matches for the student you want to request to remove";
    }
}
