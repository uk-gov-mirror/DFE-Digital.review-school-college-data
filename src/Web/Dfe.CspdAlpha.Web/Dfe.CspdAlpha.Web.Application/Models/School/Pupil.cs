using Dfe.CspdAlpha.Web.Application.Models.Common;
using System;

namespace Dfe.CspdAlpha.Web.Application.Models.School
{
    public class Pupil
    {
        public string PupilId { get; set; }
        public string Urn { get; set; }
        public string LaEstab { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfAdmission { get; set; }
        public string YearGroup { get; set; }
        public string PostCode { get; set; }

        public string Fullname => $"{FirstName.Trim()} {LastName.Trim()}";
        public string Label => $"{FirstName.Trim()} {LastName.Trim()} (Pupil ID: {PupilId})";
    }
}
