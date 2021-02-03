using System.Collections;
using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using System.Linq;
using Dfe.CspdAlpha.Web.Application.Application;
using Dfe.Rscd.Web.ApiClient;
using DateTime = System.DateTime;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil
{
    public class PupilViewModel : ContextAwareViewModel
    {
        private readonly Rscd.Web.ApiClient.Pupil _pupil;

        public PupilViewModel()
        {
            
        }

        public PupilViewModel(Rscd.Web.ApiClient.Pupil pupil)
        {
            _pupil = pupil;

            ID = pupil.Id;
            Keystage = CheckingWindow.ToKeyStage();
            URN = pupil.Urn;
            SchoolID = pupil.DfesNumber;
            UPN = pupil.Upn;
            ULN = pupil.Uln;
            FirstName = pupil.Forename;
            LastName = pupil.Surname;
            DateOfBirth = pupil.Dob.Date;
            Age = pupil.Age;
            Gender = pupil.Gender;
            DateOfAdmission = pupil.AdmissionDate.Date;
            YearGroup = pupil.YearGroup;
            AllocationYears = GetAllocationYears(pupil.Allocations);
            Allocations = pupil.Allocations;
            PincludeCode = pupil.Pincl != null ? pupil.Pincl.Code : string.Empty;
            EthnicCodeGroup = pupil.Ethnicity;
            SEN = pupil.SpecialEducationNeed;
            FirstLanguage = pupil.FirstLanguage;
            FreeSchoolsInLast6Years = pupil.FreeSchoolMealsLast6Years;
            ChildrenLookedAfter = pupil.LookedAfterEver;
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

        public string PincludeCode { get;set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }

        public string GenderString
        {
            get
            {
                if (Gender.Code == Gender.Female.Code)
                {
                    return Gender.Female.Description;
                }

                if (Gender.Code == Gender.Male.Code)
                {
                    return Gender.Male.Description;
                }

                return "Not specified";
            }
        }

        public string DateOfBirthString => DateOfBirth.ToString("dd/MM/yyyy");

        public DateTime DateOfAdmission { get; set; }
        public string YearGroup { get; set; }
        public ICollection<SourceOfAllocation> Allocations { get; set; }

        public int[] AllocationYears { get; set; }
        public string FullName => string.Join(" ", new[] {FirstName, LastName}.Where(n => !string.IsNullOrEmpty(n)));

        public SpecialEducationNeed SEN { get; set; }

        public Ethnicity EthnicCodeGroup {get;set;}
        public FirstLanguage FirstLanguage { get;set; }
        public bool FreeSchoolsInLast6Years { get; set; }
        public bool ChildrenLookedAfter { get; set; }

        public string GetPupilDetailsLabel => $"View {PersonLowercase} details";
    }
}
