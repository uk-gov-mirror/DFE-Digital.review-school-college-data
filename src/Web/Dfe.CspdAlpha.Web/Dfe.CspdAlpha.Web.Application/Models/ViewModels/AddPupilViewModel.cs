using Dfe.CspdAlpha.Web.Application.Models.Common;
using System;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels
{
    public class AddPupilViewModel
    {
        public string PupilId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DayOfBirth { get; set; }
        public int MonthOfBirth { get; set; }
        public int YearOfBirth { get; set; }
        public DateTime DateOfBirth => new DateTime(YearOfBirth, MonthOfBirth, DayOfBirth);
        public Gender Gender { get; set; }
        public int DayOfAdmission { get; set; }
        public int MonthOfAdmission { get; set; }
        public int YearOfAdmission { get; set; }
        public DateTime DateOfAdmission => new DateTime(YearOfAdmission, MonthOfAdmission, DayOfAdmission);
        public string YearGroup { get; set; }
        public string PostCode { get; set; }
    }
}
