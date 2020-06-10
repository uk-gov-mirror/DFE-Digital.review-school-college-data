using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.CspdAlpha.Web.Application.Models
{
    public class HomeViewModel
    {
        [FromForm(Name="select-a-key-stage")]
        [Required]
        public string SelectedKeyStage { get; set; }
    }
}
