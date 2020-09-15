using System.ComponentModel.DataAnnotations;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels
{
    public class HomeViewModel
    {
        [Required]
        public string SelectedKeyStage { get; set; }
    }
}
