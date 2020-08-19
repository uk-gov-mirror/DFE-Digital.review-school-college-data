using Dfe.CspdAlpha.Web.Domain.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Dfe.CspdAlpha.Web.Domain.Core;

namespace Dfe.CspdAlpha.Web.Domain.Entities
{
    public class Pupil
    {
        public PupilId Id { get; set; }
        public PupilStatus Status { get; set; }
        public URN Urn { get; set; }
        public string UPN { get; set; }
        public string LaEstab { get; set; }
        public string ForeName { get; set; }
        public string LastName { get; set; }
        public string FullName => string.Join(" ", new[] { ForeName, LastName }.Where(n => !string.IsNullOrEmpty(n)));

        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfAdmission { get; set; }
        public string YearGroup { get; set; }
        public string PostCode { get; set; }
        public bool FSM6 { get; set; }
        public List<Result> Results { get; set; }

     }
}
