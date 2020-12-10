using Dfe.CspdAlpha.Web.Application.Models.Common;
using System.Linq;
using DateTime = System.DateTime;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil
{
    public class PupilViewModel
    {
        public string ID { get; set; }
        public Keystage Keystage { get; set; }
        public string SchoolID { get; set; }
        public string URN { get; set; }
        public string UPN { get; set; }
        public string ULN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfAdmission { get; set; }
        public string YearGroup { get; set; }
        public string FullName => string.Join(" ", new[] {FirstName, LastName}.Where(n => !string.IsNullOrEmpty(n)));
        public int[] AllocationYears { get; set; }
    }
}
