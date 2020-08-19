using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using System;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments
{
    public class AmendmentViewModel
    {
        public PupilViewModel PupilViewModel { get; set; }
        //public AddPriorAttainmentViewModel AddPriorAttainmentViewModel { get; set; }

        public string PupilAge
        {
            get
            {
                if (PupilViewModel == null || PupilViewModel.DateOfBirth == DateTime.MinValue)
                {
                    return "Unknown";
                }

                var dob = PupilViewModel.DateOfBirth;
                var today = DateTime.Today;
                return (today.Year - dob.Year - (today.DayOfYear < dob.DayOfYear ? 1 : 0)).ToString();
            }
        }
    }
}
