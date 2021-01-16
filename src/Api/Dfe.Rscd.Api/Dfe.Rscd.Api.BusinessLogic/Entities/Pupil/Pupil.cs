using System;
using System.Collections.Generic;
using System.Linq;

namespace Dfe.Rscd.Api.BusinessLogic.Entities
{
    public class Pupil
    {
        public int StudentID { get; set; }

        public int? ForvusIndex { get; set; }

        public string Forename { get; set; }

        public string Surname { get; set; }

        public string ULN { get; set; }
        public string UPN { get; set; }

        public string URN { get; set; }

        public DateTime DOB { get; set; }

        public DateTime AdmissionDate { get; set; }

        public bool InCare { get; set; }

        public int Age { get; set; }

        public string YearGroup { get; set; }

        public Gender Gender { get; set; }

        public SENStatus SENStatus { get; set; }

        public FirstLanguage FirstLanguage { get; set; }

        public Ethnicity Ethnicity { get; set; }

        public FSM FSM { get; set; }

        public School School { get; set; }

        public PINCLs PINCL { get; set; }

        public string DfesNumber { get; set; }

        public string ScrutinyStatusText { get; set; }

        public PupilAggregate PupilAggregate { get; set; }

        public string DOBDisplayString { get; set; }

        public string AdmissionDateDisplayString { get; set; }

        public IList<Result> Results { get; set; }

        public IList<SourceOfAllocation> Allocations { get; set; }

        public string FullName => string.Join(" ", new[] {Forename, Surname}
            .Where(n => !string.IsNullOrEmpty(n)));
    }
}