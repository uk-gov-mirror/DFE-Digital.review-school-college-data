using System;
using System.Collections.Generic;
using System.Linq;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Microsoft.AspNetCore.Http;

namespace Dfe.CspdAlpha.Web.Application.Application.Interfaces
{
    public interface IAmendmentService
    {
        AmendmentsListViewModel GetAmendmentsListViewModel(string urn);

        AddPupilViewModel GetAddPupilAmendmentViewModel(Guid id);

        bool CancelAmendment(string id);
        List<EvidenceFile> UploadEvidence(List<IFormFile> files);

        void RelateEvidence(Guid amendmentId, List<EvidenceFile> evidenceList);

        bool CreateAddPupilAmendment(AddPupilAmendmentViewModel addPupilAmendment, out string id);
    }
}
