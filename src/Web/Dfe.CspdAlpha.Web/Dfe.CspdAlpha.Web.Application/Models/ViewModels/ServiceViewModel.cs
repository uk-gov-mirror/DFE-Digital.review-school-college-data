using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels
{
    public enum ServiceOptions
    {
        CheckData = 1,
        RemovePupil = 2,
        ViewEarlyAccess = 3
    }

    public class ServiceViewModel
    {
        [FromForm(Name = "select-a-service")]
        [Required]
        public ServiceOptions SelectedService { get; set; }
    }
}
