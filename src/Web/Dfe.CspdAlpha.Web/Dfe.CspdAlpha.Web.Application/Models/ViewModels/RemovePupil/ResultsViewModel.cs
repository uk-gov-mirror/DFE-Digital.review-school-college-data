using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil
{
    public class ResultsViewModel : ContextAwareViewModel
    {
        public ResultsViewModel() : base()
        {
            
        }

        public ResultsViewModel(string phase) : base(phase)
        {

        } 

        public string URN { get; set; }
        public QueryType SearchType { get; set; }
        public string Query { get; set; }
        public List<PupilViewModel> PupilList { get; set; }
        public string SelectedID { get; set; }
        public string PageTitle => PupilList?.Count == 0
            ? "No matches found"
            : $"We found multiple matches for the {PersonLowercase} you want to request to remove";
    }
}
