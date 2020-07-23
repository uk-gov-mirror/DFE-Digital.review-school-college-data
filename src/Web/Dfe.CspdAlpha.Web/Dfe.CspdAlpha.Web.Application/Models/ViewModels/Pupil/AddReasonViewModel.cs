using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dfe.CspdAlpha.Web.Application.Models.Common;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil
{
    public class AddReasonViewModel
    {
        public Dictionary<string, string> Reasons = new Dictionary<string, string>
        {
            { "New", "Add a new pupil (not on any school roll)"},
            {"Existing", "Add an existing pupil (previously on roll at another school)"}
        };

        [Required]
        public AddReason Reason { get; set; }
    }
}
