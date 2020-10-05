namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil
{
    public class SearchPupilsViewModel
    {
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
