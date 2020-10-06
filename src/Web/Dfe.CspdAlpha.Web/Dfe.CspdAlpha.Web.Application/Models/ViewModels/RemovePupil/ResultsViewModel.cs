using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Application.Models.Common;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil
{
    public class ResultsViewModel
    {
        public string URN { get; set; }
        public QueryType SearchType { get; set; }
        public string Query { get; set; }
        public List<PupilDetails> PupilList { get; set; }
        public string SelectedID { get; set; }
        public string PageTitle => PupilList?.Count == 0
            ? "No matches found"
            : "We found multiple matches for the student you want to request to remove";
    }
}
