using System.Collections;
using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using System.Linq;
using Dfe.CspdAlpha.Web.Application.Application;
using Dfe.Rscd.Web.ApiClient;
using DateTime = System.DateTime;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil
{
    public class PupilViewModel
    {
        private readonly Rscd.Web.ApiClient.Pupil _pupil;

        public PupilViewModel()
        {
            
        }

        public PupilViewModel(Rscd.Web.ApiClient.Pupil pupil, CheckingWindow checkingWindow)
        {
            _pupil = pupil;

            ID = pupil.Id;
            Keystage = checkingWindow.ToKeyStage();
            URN = pupil.Urn;
            SchoolID = pupil.LaEstab;
            UPN = pupil.Upn;
            ULN = pupil.Uln;
            FirstName = pupil.ForeName;
            LastName = pupil.LastName;
            DateOfBirth = pupil.DateOfBirth.Date;
            Age = pupil.Age;
            Gender = pupil.Gender;
            DateOfAdmission = pupil.DateOfAdmission.Date;
            YearGroup = pupil.YearGroup;
            AllocationYears = GetAllocationYears(pupil.Allocations);
            Allocations = pupil.Allocations;
        }

        private int[] GetAllocationYears(ICollection<SourceOfAllocation> originalAllocations)
        {
            return originalAllocations?.Select(x => x.Year)
                .OrderByDescending(x => x)
                .ToArray();
        }

        public string ID { get; set; }
        public Keystage Keystage { get; set; }
        public string SchoolID { get; set; }
        public string URN { get; set; }
        public string UPN { get; set; }
        public string ULN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfAdmission { get; set; }
        public string YearGroup { get; set; }
        public ICollection<SourceOfAllocation> Allocations { get; set; }

        public int[] AllocationYears { get; set; }
        public string FullName => string.Join(" ", new[] {FirstName, LastName}.Where(n => !string.IsNullOrEmpty(n)));

        public string GetPupilDetailsLabel => Keystage == Keystage.KS5 ? "View student details" : "View pupil details";
    }
}
