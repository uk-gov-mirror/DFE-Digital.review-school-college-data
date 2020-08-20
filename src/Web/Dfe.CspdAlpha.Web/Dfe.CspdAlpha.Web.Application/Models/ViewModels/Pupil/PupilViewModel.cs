using System;
using System.Linq;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using DateTime = System.DateTime;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil
{
    public class PupilViewModel
    {
        public string SchoolID { get; set; }
        public string UPN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? DayOfBirth { get; set; }
        public int? MonthOfBirth { get; set; }
        public int? YearOfBirth { get; set; }
        public DateTime DateOfBirth
        {
            get => new DateTime(YearOfBirth.Value, MonthOfBirth.Value, DayOfBirth.Value);
            set
            {
                DayOfBirth = value.Day;
                MonthOfBirth = value.Month;
                YearOfBirth = value.Year;
            }
        }
        public int Age
        {
            get
            {
                var now = DateTime.Now;
                var age = now.Year - DateOfBirth.Year;
                if (DateOfBirth.DayOfYear < now.DayOfYear)
                {
                    age--;
                }

                return age;
            }
        }
        public Gender Gender { get; set; }
        public int? DayOfAdmission { get; set; }
        public int? MonthOfAdmission { get; set; }
        public int? YearOfAdmission { get; set; }
        public DateTime DateOfAdmission
        {
            get => new DateTime(YearOfAdmission.Value, MonthOfAdmission.Value, DayOfAdmission.Value);
            set
            {
                DayOfAdmission = value.Day;
                MonthOfAdmission = value.Month;
                YearOfAdmission = value.Year;
            }
        }
        public string YearGroup { get; set; }
        public string FullName => string.Join(" ", new[] {FirstName, LastName}.Where(n => !string.IsNullOrEmpty(n)));
    }
}
