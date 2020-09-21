using System.Collections.Generic;

namespace Dfe.CspdAlpha.Web.Application.Models.Common
{
    public enum NavigationItem
    {
        TaskList,
        SchoolPerformance,
        PupilList,
        Amendments
    }
    public class CheckDataNavigation
    {
        public List<CheckDataNavigationItem> NavigationItems { get; set; }

        public CheckDataNavigation(NavigationItem navigationItem, CheckingWindow checkingWindow)
        {
            NavigationItems = new List<CheckDataNavigationItem>
            {
                new CheckDataNavigationItem {Label = "Task list", Controller = "TaskList", Active = navigationItem == NavigationItem.TaskList},
                new CheckDataNavigationItem {Label = GetLabel(checkingWindow), Controller = "School", Active = navigationItem == NavigationItem.SchoolPerformance, LabelClass = GetNavClass(checkingWindow)},
                new CheckDataNavigationItem {Label = "Pupil list", Controller = "Pupil", Active = navigationItem == NavigationItem.PupilList},
                new CheckDataNavigationItem {Label = "Requested amendments", Controller = "Amendments", Active = navigationItem == NavigationItem.Amendments, LabelClass = "app-ribbon-nav__list-item--tall"}
            };

             // 42959 also specifies KS2 June, but we don't have that yet 21.09.2020
            if (checkingWindow == CheckingWindow.KS4June)
            {
                NavigationItems.RemoveAt(1);
            }
        }

        private string GetLabel(CheckingWindow checkingWindow)
        {
            switch (checkingWindow)
            {
                case CheckingWindow.KS4Late:
                    return "School performance<br>summary";
                case CheckingWindow.Unknown:
                case CheckingWindow.KS2:
                case CheckingWindow.KS2Errata:
                case CheckingWindow.KS4June:
                case CheckingWindow.KS4Errata:
                case CheckingWindow.KS5:
                case CheckingWindow.KS5Errata:
                default:
                    return "School performance";
            }
        }

        private string GetNavClass(CheckingWindow checkingWindow)
        {
            switch (checkingWindow)
            {
                case CheckingWindow.KS4Late:
                    return "app-ribbon-nav__list-item--tall";
                case CheckingWindow.Unknown:
                case CheckingWindow.KS2:
                case CheckingWindow.KS2Errata:
                case CheckingWindow.KS4June:
                case CheckingWindow.KS4Errata:
                case CheckingWindow.KS5:
                case CheckingWindow.KS5Errata:
                default:
                    return string.Empty;
            }
        }
    }

    public class CheckDataNavigationItem
    {
        public string Label { get; set; }
        public string Controller { get; set; }
        public bool Active { get; set; }

        public string LabelClass { get; set; }
    }
}


