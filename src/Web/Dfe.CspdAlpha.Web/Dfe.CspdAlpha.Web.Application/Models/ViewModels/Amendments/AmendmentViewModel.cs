using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using System;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments
{
    public class AmendmentViewModel
    {
        public AddPupilViewModel AddPupilViewModel { get; set; }
        //public AddPriorAttainmentViewModel AddPriorAttainmentViewModel { get; set; }

        public string PupilAge
        {
            get
            {
                if (AddPupilViewModel == null || AddPupilViewModel.DateOfBirth == DateTime.MinValue)
                {
                    return "Unknown";
                }

                var dob = AddPupilViewModel.DateOfBirth;
                var today = DateTime.Today;
                return (today.Year - dob.Year - (today.DayOfYear < dob.DayOfYear ? 1 : 0)).ToString();
            }
        }
    }
}
