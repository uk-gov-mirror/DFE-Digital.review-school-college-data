using Dfe.Rscd.Api.Domain.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.Domain.Core;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class Pupil
    {
        public string Id { get; set; }
        public PupilStatus Status { get; set; }
        public string URN { get; set; }
        public string UPN { get; set; }
        public string ULN { get; set; }
        public string LaEstab { get; set; }
        public string ForeName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfAdmission { get; set; }
        public string YearGroup { get; set; }
        public List<Result> Results { get; set; }
        public string FullName => string.Join(" ", new[] { ForeName, LastName }.Where(n => !string.IsNullOrEmpty(n)));
    }
}
