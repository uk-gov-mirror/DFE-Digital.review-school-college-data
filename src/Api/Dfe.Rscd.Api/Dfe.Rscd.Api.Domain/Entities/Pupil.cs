using System;
using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.Domain.Core;
using Dfe.Rscd.Api.Domain.Core.Enums;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class Pupil
    {
        public string Id { get; set; }
        public PupilStatus Status { get; set; }
        public string URN { get; set; }
        public string UPN { get; set; }
        public string ULN { get; set; }
        public string DfesNumber { get; set; }
        public string ForeName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfAdmission { get; set; }
        public string YearGroup { get; set; }
        public List<Result> Results { get; set; }
        
        public List<SourceOfAllocation> Allocations { get; set; }

        public string FullName => string.Join(" ", new[] {ForeName, LastName}
            .Where(n => !string.IsNullOrEmpty(n)));


        public string AdoptedFromCareId { get; set; } 
        public int ForvusNumber { get; set; }
        public bool? FreeSchoolMeals { get; set; }
        public string SenStatus { get; set; }
        public string FirstLanguage { get; set; }
        public string Ethnicity { get; set; }
        public string PIncludeId { get; set; }
    }
}