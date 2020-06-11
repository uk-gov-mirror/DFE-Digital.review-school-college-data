using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels
{
    public class HomeViewModel
    {
        [FromForm(Name="select-a-key-stage")]
        [Required]
        public string SelectedKeyStage { get; set; }
    }
}
