namespace Dfe.Rscd.Web.Application.Models.ViewModels.RemovePupil
{
    public class SearchQuery : ContextAwareViewModel
    {
        public string URN { get; set; }
        public QueryType SearchType { get; set; }
        public string Query { get; set; }
    }
}
