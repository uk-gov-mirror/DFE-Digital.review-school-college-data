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
        public bool LateCheckingPhase { get; set; }
        public string ReviewedHeader => LateCheckingPhase ? "1. View data before requesting amendments" : "1. Review data before requesting amendments";
        public string ReviewedCopy => LateCheckingPhase ? "Viewed" : "Reviewed";
        public string NotReviewedCopy => LateCheckingPhase ? "Not viewed" : "Not reviewed";
        public string GetLeadText()
        {
            if (!ReviewChecked)
            {
                return LateCheckingPhase ?
                    "View your data before requesting amendments or confirming data." :
                    "Review your data before requesting amendments or confirming data.";
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
