using Dfe.CspdAlpha.Web.Domain.Core.Enums;
using System;
using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Domain.Core;

namespace Dfe.CspdAlpha.Web.Domain.Entities
{
    public class Pupil
    {
        public PupilId Id { get; set; }
        public PupilStatus Status { get; set; }
        public URN Urn { get; set; }
        public string ForeName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public int YearGroup { get; set; }
        public bool FSM6 { get; set; }
        public List<Result> Results { get; set; }
     }
}
