using Dfe.CspdAlpha.Web.Application.Models.Common;
using System.Collections.Generic;
using System.Linq;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil
{
    public class AllocationYearViewModel
    {
        public readonly Dictionary<int, string> AllocationYears = new Dictionary<int, string>();

        public PupilDetails PupilDetails { get; set; }

        public int[] SelectedAllocationYears { get; set; }

        public int ReasonCode { get; set; }

        public AllocationYearViewModel()
        {
            // Required to allow constructor-less creation as controller action parameter
        }

        public AllocationYearViewModel(string allocationYear)
        {

            BuildAllocationYears(allocationYear);
        }

        private void BuildAllocationYears(string allocationYear)
        {
            var year = int.Parse(allocationYear);
            var previous = year - 1;
            for (var i = 0; i < 3; i++)
            {
                AllocationYears.Add(year, $"{previous.ToString()}/{year.ToString().Substring(2)}");
                year = previous--;
            }
        }

        public string IsSelected(int allocationYear)
        {
            if(SelectedAllocationYears != null)
            {
                if(SelectedAllocationYears.ToList().Any(x => x == allocationYear)) {
                    return "checked";
                }
            }

            return string.Empty;
        }

        public string GetBackAction()
        {

            if (ReasonCode == Constants.NOT_ON_ROLL)
            {
                return "Reason";
            }
            if (ReasonCode == Constants.OTHER_EVIDENCE_NOT_REQUIRED)
            {
                return "Details";
            }
            return "Index";
        }

    }
}
