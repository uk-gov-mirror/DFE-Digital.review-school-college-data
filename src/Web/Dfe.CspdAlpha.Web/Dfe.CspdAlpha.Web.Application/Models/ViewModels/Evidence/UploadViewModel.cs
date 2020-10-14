using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Application.Models.Amendments;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Microsoft.AspNetCore.Http;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Evidence
{
    public class UploadViewModel
    {
        public AmendmentType AmendmentType { get; set; }
        public PupilDetails PupilDetails { get; set; }
        public List<IFormFile> EvidenceFiles { get; set; }
        public string ID { get; set; }

        public string GetTitle()
        {
            switch (AmendmentType)
            {
                case AmendmentType.RemovePupil:
                    return "Upload evidence";
                case AmendmentType.AddPupil:
                    return "Upload evidence " + PupilDetails.FullName;
                default:
                    return string.Empty;
            }
        }

        public string GetBackAction()
        {
            if (!string.IsNullOrWhiteSpace(ID))
            {
                return "Index";
            }

            switch (AmendmentType)
            {
                case AmendmentType.RemovePupil:
                    return "Details";
                case AmendmentType.AddPupil:
                    return "AddEvidence";
                default:
                    return string.Empty;
            }
        }
        public string GetBackController()
        {
            if (!string.IsNullOrWhiteSpace(ID))
            {
                return "Amendments";
            }
            switch (AmendmentType)
            {
                case AmendmentType.RemovePupil:
                    return "RemovePupil";
                case AmendmentType.AddPupil:
                    return "Pupil";
                default:
                    return string.Empty;
            }
        }
    }
}
