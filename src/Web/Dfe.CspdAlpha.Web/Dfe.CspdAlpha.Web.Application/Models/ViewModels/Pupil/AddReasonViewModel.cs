using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil
{
    public class AddReasonViewModel
    {
        public Dictionary<string, string> Reasons = new Dictionary<string, string>
        {
            { "Reason1", "Reason 1"},
            {"Reason2", "Reason 2"}
        };

        [Required]
        public string Reason { get; set; }
    }
}
