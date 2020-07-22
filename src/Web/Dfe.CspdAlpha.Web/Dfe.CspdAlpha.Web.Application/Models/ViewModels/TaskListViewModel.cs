using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.School;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels
{
    public class TaskListViewModel
    {
        public CheckDataNavigation CheckDataNavigationModel => new CheckDataNavigation(NavigationItem.TaskList);
        public SchoolDetails SchoolDetails { get; set; }
        public bool ReviewChecked { get; set; }
        public bool DataConfirmed { get; set; }

        public string GetLeadText()
        {
            if (!ReviewChecked)
            {
                return "Review your data before requesting amendments or confirming data.";
            }
            if (!DataConfirmed)
            {
                return "You can now request amendments or confirm data.";
            }

            return
                "You have confirmed your Key stage 4 data. You can continue to request further amendments until the end of the checking exercise window.";
        }
    }
}
