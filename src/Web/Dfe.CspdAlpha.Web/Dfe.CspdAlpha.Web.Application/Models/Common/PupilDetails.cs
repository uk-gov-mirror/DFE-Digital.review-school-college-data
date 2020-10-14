using System;
using System.Linq;

namespace Dfe.CspdAlpha.Web.Application.Models.Common
{
    public class PupilDetails
    {
        public Keystage Keystage { get; set; }
        public string ID { get; set; }
        public string UPN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfAdmission { get; set; }
        public string YearGroup { get; set; }
        public string FullName => string.Join(" ", new[] { FirstName, LastName }.Where(n => !string.IsNullOrEmpty(n)));
        public string GetPupilDetailsLabel => Keystage == Keystage.KS5 ? "View student details" : "View pupil details";
    }
}
