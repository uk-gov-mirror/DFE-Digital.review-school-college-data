using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels
{
    public class HomeViewModel
    {
        private IConfiguration ConfigRoot;
        public HomeViewModel(IConfiguration configRoot)
        {
            ConfigRoot = configRoot;
        }

        [FromForm(Name="select-a-key-stage")]
        [Required]
        public string SelectedKeyStage { get; set; }

        public string CheckingStageText => ConfigRoot["CheckingPhase"] == "Late" ? "Key stage 4 Late checking exercise" : "Key stage 4 June checking exercise";
    }
}
