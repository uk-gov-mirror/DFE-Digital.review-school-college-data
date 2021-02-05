using System;
using System.Collections.Generic;
using System.Linq;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class Pupil
    {
        public Pupil()
        {
            var notRecorded = "Not Recorded";
            SpecialEducationNeed = new SpecialEducationNeed{Code = "NR", Description = notRecorded};
            Gender = new Gender{Code = "N", Description = notRecorded};
            Ethnicity = new Ethnicity{Code = "NR", Description = notRecorded};
            FreeSchoolMealsLast6Years = false;
            Allocations = new List<SourceOfAllocation>();
            PINCL = new PInclude{Code = "NR", Description = notRecorded};
            FirstLanguage = new FirstLanguage { Code="NR", Description = notRecorded};
            School = new School();
            Results = new List<Result>();
        }

        public string Id { get; set; }

        public int? ForvusIndex { get; set; }

        public string Forename { get; set; }

        public string Surname { get; set; }

        public string ULN { get; set; }
        public string UPN { get; set; }

        public string URN { get; set; }

        public DateTime DOB { get; set; }

        public DateTime AdmissionDate { get; set; }

        public bool LookedAfterEver { get; set; }

        public int Age { get; set; }

        public string YearGroup { get; set; }

        public Gender Gender { get; set; }

        public SpecialEducationNeed SpecialEducationNeed { get; set; }

        public FirstLanguage FirstLanguage { get; set; }

        public Ethnicity Ethnicity { get; set; }

        public bool FreeSchoolMealsLast6Years { get; set; }

        public School School { get; set; }

        public PInclude PINCL { get; set; }

        public string DfesNumber { get; set; }

        public string ScrutinyStatusText { get; set; }

        public string DOBDisplayString { get; set; }

        public string AdmissionDateDisplayString { get; set; }

        public IList<Result> Results { get; set; }

        public IList<SourceOfAllocation> Allocations { get; set; }

        public string FullName => string.Join(" ", new[] {Forename, Surname}
            .Where(n => !string.IsNullOrEmpty(n)));
    }
}