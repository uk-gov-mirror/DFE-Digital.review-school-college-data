using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.CspdAlpha.Web.Application.Models.Common;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.AddPupil
{
    public class AddPupilViewModel
    {
        public string SchoolID { get; set; }

        public string UPN { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateViewModel DateOfBirth { get; set; }

        public Gender? Gender { get; set; }

        public DateViewModel DateOfAdmission { get; set; }

        public string YearGroup { get; set; }
    }
}
