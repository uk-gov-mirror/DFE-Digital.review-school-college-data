using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.CspdAlpha.Web.Application.Models
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
