using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.CspdAlpha.Web.Application.Models.Common;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil
{
    public class UploadEvidenceViewModel
    {
        public string Id { get; set; }
        public AddPupilViewModel AddPupilViewModel { get; set; }
        public List<EvidenceFile> EvidenceFiles { get; set; }
    }
}
