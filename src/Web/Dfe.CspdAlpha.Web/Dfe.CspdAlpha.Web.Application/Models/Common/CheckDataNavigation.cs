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

        public CheckDataNavigation(NavigationItem navigationItem)
        {
            NavigationItems = new List<CheckDataNavigationItem>
            {
                new CheckDataNavigationItem {Label = "Task list", Controller = "TaskList", Active = navigationItem == NavigationItem.TaskList},
                new CheckDataNavigationItem {Label = "School performance", Controller = "School", Active = navigationItem == NavigationItem.SchoolPerformance},
                new CheckDataNavigationItem {Label = "Pupil list", Controller = "Pupil", Active = navigationItem == NavigationItem.PupilList},
                new CheckDataNavigationItem {Label = "Amendments", Controller = "Amendments", Active = navigationItem == NavigationItem.Amendments}
            };
        }
    }

    public class CheckDataNavigationItem
    {
        public string Label { get; set; }
        public string Controller { get; set; }
        public bool Active { get; set; }
    }
}
