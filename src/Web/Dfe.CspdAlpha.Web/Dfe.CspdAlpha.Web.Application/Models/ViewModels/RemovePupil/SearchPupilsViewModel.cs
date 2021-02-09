namespace Dfe.Rscd.Web.Application.Models.ViewModels.RemovePupil
{
    public class SearchPupilsViewModel : ContextAwareViewModel
    {
        public SearchPupilsViewModel() : base()
        {
            
        }

        public SearchPupilsViewModel(string phase) : base(phase)
        {
            
        }

        public string PupilID { get; set; }
        public string Name { get; set; }
        public QueryType SearchType { get; set; }
    }

    public enum QueryType
    {
        None,
        PupilID,
        Name
    }
}
