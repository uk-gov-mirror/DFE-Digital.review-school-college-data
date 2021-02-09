using System.Collections.Generic;
using Dfe.Rscd.Web.ApiClient;

namespace Dfe.Rscd.Web.Application.Models.Common
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
                new CheckDataNavigationItem {Label = GetSchoolPerformanceLabel(checkingWindow), Controller = "School", Active = navigationItem == NavigationItem.SchoolPerformance, LabelClass = GetNavClass(checkingWindow)},
                new CheckDataNavigationItem {Label = GetPupilListLabel(checkingWindow), Controller = "Pupil", Active = navigationItem == NavigationItem.PupilList},
                new CheckDataNavigationItem {Label = "Requested amendments", Controller = "Amendments", Active = navigationItem == NavigationItem.Amendments, LabelClass = "app-ribbon-nav__list-item--tall"}
            };

             // 42959 also specifies KS2 June, but we don't have that yet 21.09.2020
            if (checkingWindow == CheckingWindow.KS4June)
            {
                NavigationItems.RemoveAt(1);
            }
        }

        private string GetSchoolPerformanceLabel(CheckingWindow checkingWindow)
        {
            switch (checkingWindow)
            {
                case CheckingWindow.KS4Late:
                case CheckingWindow.KS5:
                    return "School performance<br>summary";
                case CheckingWindow.Unknown:
                case CheckingWindow.KS2:
                case CheckingWindow.KS2Errata:
                case CheckingWindow.KS4June:
                case CheckingWindow.KS4Errata:
                case CheckingWindow.KS5Errata:
                default:
                    return "School performance";
            }
        }
        private string GetPupilListLabel(CheckingWindow checkingWindow)
        {
            switch (checkingWindow)
            {
                case CheckingWindow.KS5:
                    return "Student list";
                case CheckingWindow.Unknown:
                case CheckingWindow.KS2:
                case CheckingWindow.KS2Errata:
                case CheckingWindow.KS4June:
                case CheckingWindow.KS4Late:
                case CheckingWindow.KS4Errata:
                case CheckingWindow.KS5Errata:
                default:
                    return "Pupil list";
            }
        }

        private string GetNavClass(CheckingWindow checkingWindow)
        {
            switch (checkingWindow)
            {
                case CheckingWindow.KS4Late:
                case CheckingWindow.KS5:
                    return "app-ribbon-nav__list-item--tall";
                case CheckingWindow.Unknown:
                case CheckingWindow.KS2:
                case CheckingWindow.KS2Errata:
                case CheckingWindow.KS4June:
                case CheckingWindow.KS4Errata:
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


