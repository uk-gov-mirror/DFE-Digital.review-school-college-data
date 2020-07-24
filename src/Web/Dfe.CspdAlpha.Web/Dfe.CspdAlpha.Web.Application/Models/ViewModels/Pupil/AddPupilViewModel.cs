using System;
using System.Linq;
using Dfe.CspdAlpha.Web.Application.Models.Common;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil
{
    public class AddPupilViewModel
    {
        public string PupilId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? DayOfBirth { get; set; }
        public int? MonthOfBirth { get; set; }
        public int? YearOfBirth { get; set; }
        public DateTime DateOfBirth => new DateTime(YearOfBirth.Value, MonthOfBirth.Value, DayOfBirth.Value);
        public Gender Gender { get; set; }
        public int? DayOfAdmission { get; set; }
        public int? MonthOfAdmission { get; set; }
        public int? YearOfAdmission { get; set; }
        public DateTime DateOfAdmission => new DateTime(YearOfAdmission.Value, MonthOfAdmission.Value, DayOfAdmission.Value);
        public string YearGroup { get; set; }
        public string PostCode { get; set; }

        public string FullName => string.Join(" ", new[] {FirstName, LastName}.Where(n => !string.IsNullOrEmpty(n)));

        public AddReason AddReason { get; set; }
    }
}
