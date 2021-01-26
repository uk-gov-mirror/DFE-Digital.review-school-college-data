using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil
{
    public class RemovePupilViewModel : ContextAwareViewModel
    {
        public PupilViewModel PupilViewModel { get; set; }
        public QueryType QueryType { get; set; }
        public string query { get; set; }
    }
}
