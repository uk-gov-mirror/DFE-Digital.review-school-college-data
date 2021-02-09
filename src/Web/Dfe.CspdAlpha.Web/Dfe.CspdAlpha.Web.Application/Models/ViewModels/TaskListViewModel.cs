using System;
using Dfe.Rscd.Web.Application.Models.Common;
using Dfe.Rscd.Web.Application.Models.School;

namespace Dfe.Rscd.Web.Application.Models.ViewModels
{
    public class TaskListViewModel : ContextAwareViewModel
    {
        public TaskListViewModel(string phase) : base(phase) { }

        public TaskListViewModel() : base()
        {
            
        }

        public CheckDataNavigation CheckDataNavigationModel => new CheckDataNavigation(NavigationItem.TaskList , CheckingWindow);
        public SchoolDetails SchoolDetails { get; set; }
        public bool ReviewChecked { get; set; }
        public bool DataConfirmed { get; set; }
        public DateTime ConfirmationDate { get; set; }

        public string ReviewedHeader = "1. View data before requesting amendments";

        public string ReviewedCopy = "Viewed";

        public string NotReviewedCopy = "Not viewed";
        public string GetLeadText()
        {
            if (!ReviewChecked)
            {
                return "View your data before requesting amendments or confirming data.";
            }
            if (!DataConfirmed)
            {
                return "You can now request amendments or confirm data.";
            }

            var checkingWindow = CheckingWindow.ToString();
            if (checkingWindow.StartsWith("KS2"))
            {
                return
                    $"You have confirmed your Key stage 2 data on {ConfirmationDate:dd/MM/yyyy}. You can continue to request further amendments until the end of the checking exercise window.";
            }
            if (checkingWindow.StartsWith("KS4"))
            {
                return
                    $"You have confirmed your Key stage 4 data on {ConfirmationDate:dd/MM/yyyy}. You can continue to request further amendments until the end of the checking exercise window.";
            }
            return
                $"You have confirmed your data. You can continue to request further amendments until the end of the checking exercise window.";
        }

        public string GetReviewedRowText()
        {
            var checkingWindow = CheckingWindow.ToString();
            if (checkingWindow.StartsWith("KS2"))
            {
                return
                    $"You have confirmed your Key stage 2 data on {ConfirmationDate:dd/MM/yyyy}. You can continue to request further amendments until the end of the checking exercise window.";
            }
            if (checkingWindow.StartsWith("KS4"))
            {
                return
                    $"You have confirmed your Key stage 4 data on {ConfirmationDate:dd/MM/yyyy}. You can continue to request further amendments until the end of the checking exercise window.";
            }
            return
                $"You have confirmed your 16 to 18 data on {ConfirmationDate:dd/MM/yyyy}. You can continue to request further amendments until the end of the checking exercise window.";
        }
    }
}
