using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public CheckDataNavigation(NavigationItem navigationItem, bool lateChecking = false)
        {
            var performanceLabel = lateChecking ? "School performance<br>summary" : "School performance";
            var performanceNavClass = lateChecking ? "app-ribbon-nav__list-item--tall" : String.Empty;
            
            NavigationItems = new List<CheckDataNavigationItem>
            {
                new CheckDataNavigationItem {Label = "Task list", Controller = "TaskList", Active = navigationItem == NavigationItem.TaskList},
                new CheckDataNavigationItem {Label = performanceLabel, Controller = "School", Active = navigationItem == NavigationItem.SchoolPerformance, LabelClass = performanceNavClass},
                new CheckDataNavigationItem {Label = "Pupil list", Controller = "Pupil", Active = navigationItem == NavigationItem.PupilList},
                new CheckDataNavigationItem {Label = "Requested<br>amendments", Controller = "Amendments", Active = navigationItem == NavigationItem.Amendments, LabelClass = "app-ribbon-nav__list-item--tall"}
            };
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
