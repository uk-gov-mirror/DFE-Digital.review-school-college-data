using System.ComponentModel.DataAnnotations;

namespace Dfe.Rscd.Web.Application.Models.ViewModels
{
    public class HomeViewModel
    {
        [Required]
        public string SelectedKeyStage { get; set; }
    }
}
